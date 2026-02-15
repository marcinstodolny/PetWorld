using Microsoft.Agents.AI;

namespace PetWorld.Infrastructure.Services.WriterCritic.Agents;

public interface IWriterCriticAgentFactory
{
    ChatClientAgent CreateWriterAgent(string apiKey, string model);
    ChatClientAgent CreateCriticAgent(string apiKey, string model, int feedbackMaxLength);
}
