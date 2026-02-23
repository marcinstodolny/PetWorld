namespace PetWorld.Application.Configuration;

public sealed class AgentFrameworkOptions
{
    public const string SectionName = "AgentFramework";

    public string? OpenAiApiKey { get; init; }
    public string DefaultModel { get; init; } = "gpt-5-nano";
    public List<string> AvailableModels { get; init; } = ["gpt-5-nano"];
}
