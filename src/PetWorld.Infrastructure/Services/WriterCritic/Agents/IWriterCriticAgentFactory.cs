using Microsoft.Agents.AI;

namespace PetWorld.Infrastructure.Services.WriterCritic.Agents;

public interface IWriterCriticAgentFactory
{
    ChatClientAgent CreateWriterAgent(string apiKey, string? modelOverride = null);
    ChatClientAgent CreateCriticAgent(string apiKey, string? modelOverride, int feedbackMaxLength);
}
