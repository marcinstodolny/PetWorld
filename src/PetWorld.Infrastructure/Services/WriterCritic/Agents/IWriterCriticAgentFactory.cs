using Microsoft.Agents.AI;

namespace PetWorld.Infrastructure.Services.WriterCritic.Agents;

public interface IWriterCriticAgentFactory
{
    ChatClientAgent CreateWriterAgent(string apiKey);
    ChatClientAgent CreateCriticAgent(string apiKey, int feedbackMaxLength);
}
