using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using OpenAI.Chat;
using PetWorld.Application.Abstraction.AI;
using PetWorld.Application.Models;
using PetWorld.Domain.Base;
using PetWorld.Domain.Entities;
using PetWorld.Domain.Enums;
using PetWorld.Domain.ValueObjects;
using System.Text;
using System.Text.Json;
using PetWorld.Infrastructure.Services.WriterCritic.Prompts;

namespace PetWorld.Infrastructure.Services.WriterCritic;

public sealed class WriterCriticService(
    IOptions<AgentFrameworkOptions> options,
    IWriterBuilder writerBuilder,
    ICriticBuilder criticBuilder) : IWriterCriticService
{
    private const int FeedbackMaxLength = 200;
    private const string DefaultFeedback = "Odpowiedź wymaga poprawy. Ulepsz rekomendacje produktów i dopasuj je do pytania.";
    private readonly AgentFrameworkOptions _options = options.Value;

    private static readonly JsonSerializerOptions SJsonOptions = new() { PropertyNameCaseInsensitive = true };

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
        var productCatalog = BuildCatalog(products);
        var writerAgent = CreateWriterAgent(apiKey);
        var criticAgent = CreateCriticAgent(apiKey);

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

            var (approved, Feedback) = ParseCriticResponse(criticResponseResult.Value);
            feedback = Feedback;

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

    private ChatClientAgent CreateWriterAgent(string apiKey)
    {
        return CreateChatClient(apiKey)
            .AsAIAgent(name: "Writer",
                instructions: writerBuilder.BuildInstructions());
    }


    private ChatClientAgent CreateCriticAgent(string apiKey)
    {
        return CreateChatClient(apiKey)
            .AsAIAgent(
                name: "Critic",
                instructions: criticBuilder.BuildInstructions(FeedbackMaxLength));
    }


    private IChatClient CreateChatClient(string apiKey)
    {
        return new ChatClient(_options.Model, apiKey).AsIChatClient();
    }

    private string? GetApiKey()
    {
        return string.IsNullOrWhiteSpace(_options.OpenAiApiKey)
            ? Environment.GetEnvironmentVariable("OPENAI_API_KEY")
            : _options.OpenAiApiKey;
    }

    private static (bool Approved, string Feedback) ParseCriticResponse(string response)
    {
        try
        {
            var json = ExtractJson(response);

            var parsed = JsonSerializer.Deserialize<CriticResponse>(json, SJsonOptions);
            if (parsed is not null)
            {
                var feedback = NormalizeFeedback(parsed.Feedback);
                return (parsed.Approved, feedback);
            }
        }
        catch (JsonException)
        {
            // ignore
        }

        return (false, DefaultFeedback);
    }

    private static string NormalizeFeedback(string? feedback)
    {
        var normalized = string.IsNullOrWhiteSpace(feedback)
            ? DefaultFeedback
            : feedback.Trim();

        return normalized.Length <= FeedbackMaxLength
            ? normalized
            : normalized[..FeedbackMaxLength];
    }

    private static string ExtractJson(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new JsonException("Empty response.");

        var trimmed = text.Trim();

        while (trimmed.StartsWith("{{") && trimmed.EndsWith("}}"))
            trimmed = trimmed[1..^1].Trim();

        var start = trimmed.IndexOf('{');
        var end = trimmed.LastIndexOf('}');
        if (start < 0 || end <= start)
            throw new JsonException("No JSON object found.");

        return trimmed.Substring(start, end - start + 1);
    }

    private static string BuildCatalog(IReadOnlyList<Product> products)
    {
        if (products.Count == 0)
        {
            return "Brak produktów w katalogu.";
        }

        var builder = new StringBuilder();
        var index = 1;

        foreach (var product in products)
        {
            builder.AppendLine(
                $"{index}) \"{product.Name.Value}\" | Kategoria: {product.Category.ToPolish()} | Cena: {product.Price.Amount} {product.Price.Currency} | Opis: {product.Description.Value}");
            index++;
        }

        return builder.ToString().Trim();
    }

    private sealed record CriticResponse(bool Approved, string? Feedback);
}
