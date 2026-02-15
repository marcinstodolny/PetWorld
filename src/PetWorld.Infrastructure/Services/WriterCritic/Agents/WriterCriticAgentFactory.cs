using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using OpenAI.Chat;
using PetWorld.Infrastructure.Services.WriterCritic.Prompts;

namespace PetWorld.Infrastructure.Services.WriterCritic.Agents;

public sealed class WriterCriticAgentFactory(
    IOptions<AgentFrameworkOptions> options,
    IWriterBuilder writerBuilder,
    ICriticBuilder criticBuilder) : IWriterCriticAgentFactory
{
    private readonly AgentFrameworkOptions _options = options.Value;

    public ChatClientAgent CreateWriterAgent(string apiKey)
    {
        return CreateChatClient(apiKey)
            .AsAIAgent(
                name: "Writer",
                instructions: writerBuilder.BuildInstructions());
    }

    public ChatClientAgent CreateCriticAgent(string apiKey, int feedbackMaxLength)
    {
        return CreateChatClient(apiKey)
            .AsAIAgent(
                name: "Critic",
                instructions: criticBuilder.BuildInstructions(feedbackMaxLength));
    }

    private IChatClient CreateChatClient(string apiKey)
    {
        return new ChatClient(_options.Model, apiKey).AsIChatClient();
    }
}
