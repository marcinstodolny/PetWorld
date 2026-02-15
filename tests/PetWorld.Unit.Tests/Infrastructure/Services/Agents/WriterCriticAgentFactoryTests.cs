using Microsoft.Extensions.Options;
using PetWorld.Infrastructure.Services.WriterCritic;
using PetWorld.Infrastructure.Services.WriterCritic.Agents;
using PetWorld.Infrastructure.Services.WriterCritic.Prompts;

namespace PetWorld.Unit.Tests.Infrastructure.Services.Agents;

public class WriterCriticAgentFactoryTests
{
    [Fact]
    public void CreateWriterAgent_ShouldUseWriterNameAndInstructions()
    {
        var writerBuilder = new FakeWriterBuilder("writer-instructions");
        var criticBuilder = new FakeCriticBuilder("critic-instructions");
        var options = Options.Create(new AgentFrameworkOptions
        {
            Model = "gpt-4o-mini",
            OpenAiApiKey = "test-key"
        });

        var factory = new WriterCriticAgentFactory(options, writerBuilder, criticBuilder);

        var agent = factory.CreateWriterAgent("api-key");

        Assert.Equal("Writer", agent.Name);
        Assert.Equal(1, writerBuilder.BuildInstructionsCalls);
        Assert.Equal(0, criticBuilder.BuildInstructionsCalls);
    }

    [Fact]
    public void CreateCriticAgent_ShouldUseCriticNameAndFeedbackLimit()
    {
        var writerBuilder = new FakeWriterBuilder("writer-instructions");
        var criticBuilder = new FakeCriticBuilder("critic-instructions");
        var options = Options.Create(new AgentFrameworkOptions
        {
            Model = "gpt-4o-mini",
            OpenAiApiKey = "test-key"
        });

        var factory = new WriterCriticAgentFactory(options, writerBuilder, criticBuilder);

        var agent = factory.CreateCriticAgent("api-key", 200);

        Assert.Equal("Critic", agent.Name);
        Assert.Equal(1, criticBuilder.BuildInstructionsCalls);
        Assert.Equal(200, criticBuilder.LastFeedbackMaxLength);
        Assert.Equal(0, writerBuilder.BuildInstructionsCalls);
    }

    private sealed class FakeWriterBuilder(string instructions) : IWriterBuilder
    {
        public int BuildInstructionsCalls { get; private set; }

        public string BuildPrompt(string question, string catalog, string? feedback)
        {
            return string.Empty;
        }

        public string BuildInstructions()
        {
            BuildInstructionsCalls++;
            return instructions;
        }
    }

    private sealed class FakeCriticBuilder(string instructions) : ICriticBuilder
    {
        public int BuildInstructionsCalls { get; private set; }
        public int? LastFeedbackMaxLength { get; private set; }

        public string BuildPrompt(string question, string answer, string catalog)
        {
            return string.Empty;
        }

        public string BuildInstructions(int feedbackMaxLength)
        {
            BuildInstructionsCalls++;
            LastFeedbackMaxLength = feedbackMaxLength;
            return instructions;
        }
    }
}
