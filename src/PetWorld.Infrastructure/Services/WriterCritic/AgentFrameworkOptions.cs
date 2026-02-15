namespace PetWorld.Infrastructure.Services.WriterCritic;

public sealed class AgentFrameworkOptions
{
    public const string SectionName = "AgentFramework";

    public string? OpenAiApiKey { get; init; }
    public string Model { get; init; } = "gpt-5-nano";
}
