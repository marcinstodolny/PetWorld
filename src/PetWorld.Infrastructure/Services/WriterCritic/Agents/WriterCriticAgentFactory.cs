using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using OpenAI.Chat;
using PetWorld.Infrastructure.Services.WriterCritic.Prompts;

namespace PetWorld.Infrastructure.Services.WriterCritic.Agents;

public sealed class WriterCriticAgentFactory : IWriterCriticAgentFactory
{
    private readonly IWriterBuilder _writerBuilder;
    private readonly ICriticBuilder _criticBuilder;
    private readonly string _defaultModel;
    private readonly Dictionary<string, string> _allowedModelsMap;

    public WriterCriticAgentFactory(
        IWriterBuilder writerBuilder,
        ICriticBuilder criticBuilder,
        IOptions<AgentFrameworkOptions> options)
    {
        _writerBuilder = writerBuilder;
        _criticBuilder = criticBuilder;

        _defaultModel = NormalizeDefaultModel(options.Value);
        _allowedModelsMap = BuildAllowedModelsMap(options.Value, _defaultModel) ;
    }

    public ChatClientAgent CreateWriterAgent(string apiKey, string? modelOverride = null)
    {
        var modelToUse = ResolveModelToUse(modelOverride);

        return CreateChatClient(apiKey, modelToUse)
            .AsAIAgent(
                name: "Writer",
                instructions: _writerBuilder.BuildInstructions());
    }

    public ChatClientAgent CreateCriticAgent(string apiKey, string? modelOverride, int feedbackMaxLength)
    {
        var modelToUse = ResolveModelToUse(modelOverride);

        return CreateChatClient(apiKey, modelToUse)
            .AsAIAgent(
                name: "Critic",
                instructions: _criticBuilder.BuildInstructions(feedbackMaxLength));
    }

    internal string ResolveModelToUse(string? modelOverride)
    {
        var normalizedOverride = modelOverride?.Trim();
        return string.IsNullOrWhiteSpace(normalizedOverride) ? _defaultModel : _allowedModelsMap.GetValueOrDefault(normalizedOverride, _defaultModel);
    }

    private static IChatClient CreateChatClient(string apiKey, string model)
    {
        return new ChatClient(model, apiKey).AsIChatClient();
    }

    private static string NormalizeDefaultModel(AgentFrameworkOptions options)
    {
        var normalizedDefaultModel = options.DefaultModel?.Trim();

        return string.IsNullOrWhiteSpace(normalizedDefaultModel)
            ? new AgentFrameworkOptions().DefaultModel
            : normalizedDefaultModel;
    }

    private static Dictionary<string, string> BuildAllowedModelsMap(AgentFrameworkOptions options, string defaultModel)
    {
        var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        foreach (var configuredModel in options.AvailableModels ?? [])
        {
            var normalized = configuredModel.Trim();
            if (string.IsNullOrWhiteSpace(normalized))
            {
                continue;
            }

            map.TryAdd(normalized, normalized);
        }

        map.TryAdd(defaultModel, defaultModel);

        return map;
    }
}
