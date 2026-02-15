using Microsoft.Agents.AI;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PetWorld.Application.Abstraction.AI;
using PetWorld.Application.Models;
using PetWorld.Domain.Base;
using PetWorld.Domain.Entities;
using PetWorld.Domain.ValueObjects;
using PetWorld.Infrastructure.Services.WriterCritic.Agents;
using PetWorld.Infrastructure.Services.WriterCritic.Catalog;
using PetWorld.Infrastructure.Services.WriterCritic.Parsing;
using PetWorld.Infrastructure.Services.WriterCritic.Prompts;

namespace PetWorld.Infrastructure.Services.WriterCritic;

public sealed class WriterCriticService(
    IOptions<AgentFrameworkOptions> options,
    IWriterBuilder writerBuilder,
    ICriticBuilder criticBuilder,
    IProductCatalogBuilder catalogBuilder,
    ICriticResponseParser criticResponseParser,
    IWriterCriticAgentFactory agentFactory,
    ILogger<WriterCriticService> logger) : IWriterCriticService
{
    private readonly AgentFrameworkOptions _options = options.Value;

    public const int FeedbackMaxLength = 200;
    public const string DefaultFeedback = "Odpowiedź wymaga poprawy. Ulepsz rekomendacje produktów i dopasuj je do pytania.";
    private const string ModelUnavailableUserErrorMessage = "Wybrany model jest niedostępny dla tego klucza OpenAI. Wybierz inny model i spróbuj ponownie.";

    public async Task<Result<WriterCriticResult>> GenerateResponseAsync(
        string question,
        IReadOnlyList<Product> products,
        string? modelOverride = null,
        CancellationToken cancellationToken = default)
    {
        var apiKey = GetApiKey();
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            return Result.Fail<WriterCriticResult>("Brak klucza OpenAI. Ustaw AgentFramework:OpenAiApiKey w appsettings lub OPENAI_API_KEY w zmiennych środowiskowych. (patrz README)");
        }

        var modelToUse = ResolveModel(modelOverride);
        return await GenerateResponseInternalAsync(question, products, apiKey, modelToUse, cancellationToken);
    }

    private async Task<Result<WriterCriticResult>> GenerateResponseInternalAsync(string question, IReadOnlyList<Product> products, string apiKey, string modelToUse, CancellationToken cancellationToken)
    {
        var productCatalog = catalogBuilder.Build(products);
        var writerAgent = agentFactory.CreateWriterAgent(apiKey, modelToUse);
        var criticAgent = agentFactory.CreateCriticAgent(apiKey, modelToUse, FeedbackMaxLength);

        string? feedback = null;
        string? lastAnswer = null;
        var iterationsCompleted = 0;

        for (var iteration = 1; iteration <= IterationCount.MaxValue; iteration++)
        {
            var writerPrompt = writerBuilder.BuildPrompt(question, productCatalog, feedback);
            var writerResponseResult = await ExecuteAgentAsync(writerAgent, writerPrompt, iteration, logger, cancellationToken);
            if (writerResponseResult.IsFailed)
            {
                return Result.Fail<WriterCriticResult>(writerResponseResult.Errors);
            }

            var answer = writerResponseResult.Value;
            lastAnswer = answer;
            iterationsCompleted = iteration;

            var criticPrompt = criticBuilder.BuildPrompt(question, answer, productCatalog);
            var criticResponseResult = await ExecuteAgentAsync(criticAgent, criticPrompt, iteration, logger, cancellationToken);
            if (criticResponseResult.IsFailed)
            {
                return Result.Fail<WriterCriticResult>(criticResponseResult.Errors);
            }

            var (approved, parsedFeedback) = criticResponseParser.Parse(criticResponseResult.Value, FeedbackMaxLength, DefaultFeedback);
            feedback = parsedFeedback;

            if (approved)
            {
                return Result.Success(new WriterCriticResult(answer, iteration));
            }
        }

        return Result.Success(new WriterCriticResult(lastAnswer ?? string.Empty, iterationsCompleted));
    }

    private static async Task<Result<string>> ExecuteAgentAsync(ChatClientAgent agent, string prompt, int iteration, ILogger<WriterCriticService> logger, CancellationToken cancellationToken)
    {
        try
        {
            var response = await agent.RunAsync(prompt, session: null, options: null, cancellationToken);
            return Result.Success(response.Text);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "AI error. Stage={Stage}, Iteration={Iteration}", agent.Name, iteration);
            return Result.Fail<string>(IsModelUnavailableError(ex) ? ModelUnavailableUserErrorMessage : "Wystąpił błąd usługi AI. Spróbuj ponownie.");
        }
    }

    private string ResolveModel(string? modelOverride)
    {
        if (string.IsNullOrWhiteSpace(modelOverride))
        {
            return _options.DefaultModel;
        }

        var availableModels = GetAvailableModels();
        var matchedModel = availableModels.FirstOrDefault(model => string.Equals(model, modelOverride, StringComparison.Ordinal));

        return matchedModel ?? _options.DefaultModel;
    }

    private List<string> GetAvailableModels()
    {
        var configured = _options.AvailableModels
            .Where(model => !string.IsNullOrWhiteSpace(model))
            .Select(model => model.Trim())
            .Distinct(StringComparer.Ordinal)
            .ToList();

        if (configured.Count == 0)
        {
            configured.Add(_options.DefaultModel);
        }

        return configured;
    }

    private static bool IsModelUnavailableError(Exception ex)
    {
        var message = ex.Message;
        if (string.IsNullOrWhiteSpace(message))
        {
            return false;
        }

        return message.Contains("model_not_found", StringComparison.OrdinalIgnoreCase)
            || message.Contains("model not found", StringComparison.OrdinalIgnoreCase)
            || message.Contains("not allowed", StringComparison.OrdinalIgnoreCase)
            || message.Contains("403", StringComparison.OrdinalIgnoreCase)
            || message.Contains("404", StringComparison.OrdinalIgnoreCase);
    }

    private string? GetApiKey()
    {
        return string.IsNullOrWhiteSpace(_options.OpenAiApiKey)
            ? Environment.GetEnvironmentVariable("OPENAI_API_KEY")
            : _options.OpenAiApiKey;
    }
}
