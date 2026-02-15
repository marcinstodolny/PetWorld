using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using OpenAI.Chat;
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
    IWriterCriticAgentFactory agentFactory) : IWriterCriticService
{
    private readonly AgentFrameworkOptions _options = options.Value;

    public const int FeedbackMaxLength = 200;
    public const string DefaultFeedback = "Odpowiedź wymaga poprawy. Ulepsz rekomendacje produktów i dopasuj je do pytania.";

    public async Task<Result<WriterCriticResult>> GenerateResponseAsync(string question, IReadOnlyList<Product> products, CancellationToken cancellationToken = default)
    {
        var apiKey = GetApiKey();
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            return Result.Fail<WriterCriticResult>("Brak klucza OpenAI. Ustaw AgentFramework:OpenAiApiKey w appsettings lub OPENAI_API_KEY w zmiennych środowiskowych.");
        }

        return await GenerateResponseInternalAsync(question, products, apiKey, cancellationToken);
    }

    private async Task<Result<WriterCriticResult>> GenerateResponseInternalAsync(string question, IReadOnlyList<Product> products, string apiKey, CancellationToken cancellationToken)
    {
        var productCatalog = catalogBuilder.Build(products);
        var writerAgent = agentFactory.CreateWriterAgent(apiKey);
        var criticAgent = agentFactory.CreateCriticAgent(apiKey, FeedbackMaxLength);

        string? feedback = null;
        string? lastAnswer = null;
        var iterationsCompleted = 0;

        for (var iteration = 1; iteration <= IterationCount.MaxValue; iteration++)
        {
            var writerPrompt = writerBuilder.BuildPrompt(question, productCatalog, feedback);
            var writerResponseResult = await ExecuteAgentAsync(writerAgent, writerPrompt, iteration, cancellationToken);
            if (writerResponseResult.IsFailed)
            {
                return Result.Fail<WriterCriticResult>(writerResponseResult.Errors);
            }

            var answer = writerResponseResult.Value;
            lastAnswer = answer;
            iterationsCompleted = iteration;

            var criticPrompt = criticBuilder.BuildPrompt(question, answer, productCatalog);
            var criticResponseResult = await ExecuteAgentAsync(criticAgent, criticPrompt, iteration, cancellationToken);
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

    private static async Task<Result<string>> ExecuteAgentAsync(ChatClientAgent agent, string prompt, int iteration, CancellationToken cancellationToken)
    {
        try
        {
            var response = await agent.RunAsync(prompt, session: null, options: null, cancellationToken);
            return Result.Success(response.Text);
        }
        catch (OperationCanceledException)
        {
            var message = $"Operacja przerwana na etapie '{agent.Name}', iteracja {iteration}.";
            return Result.Fail<string>(message);
        }
        catch (Exception ex)
        {
            var message = $"Błąd modelu AI na etapie '{agent.Name}', iteracja {iteration}: {ex.Message}";
            return Result.Fail<string>(message);
        }
    }

    private string? GetApiKey()
    {
        return string.IsNullOrWhiteSpace(_options.OpenAiApiKey)
            ? Environment.GetEnvironmentVariable("OPENAI_API_KEY")
            : _options.OpenAiApiKey;
    }
}
