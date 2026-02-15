using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI.Chat;
using PetWorld.Infrastructure.Services.WriterCritic.Prompts;

namespace PetWorld.Infrastructure.Services.WriterCritic.Agents;

public sealed class WriterCriticAgentFactory(
    IWriterBuilder writerBuilder,
    ICriticBuilder criticBuilder) : IWriterCriticAgentFactory
{
    public ChatClientAgent CreateWriterAgent(string apiKey, string model)
    {
        return CreateChatClient(apiKey, model)
            .AsAIAgent(
                name: "Writer",
                instructions: writerBuilder.BuildInstructions());
    }

    public ChatClientAgent CreateCriticAgent(string apiKey, string model, int feedbackMaxLength)
    {
        return CreateChatClient(apiKey, model)
            .AsAIAgent(
                name: "Critic",
                instructions: criticBuilder.BuildInstructions(feedbackMaxLength));
    }

    private static IChatClient CreateChatClient(string apiKey, string model)
    {
        return new ChatClient(model, apiKey).AsIChatClient();
    }
}
