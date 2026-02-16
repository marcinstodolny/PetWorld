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
        var factory = CreateFactory(writerBuilder, criticBuilder);

        var agent = factory.CreateWriterAgent("api-key", "gpt-4o-mini");

        Assert.Equal("Writer", agent.Name);
        Assert.Equal(1, writerBuilder.BuildInstructionsCalls);
        Assert.Equal(0, criticBuilder.BuildInstructionsCalls);
    }

    [Fact]
    public void CreateCriticAgent_ShouldUseCriticNameAndFeedbackLimit()
    {
        var writerBuilder = new FakeWriterBuilder("writer-instructions");
        var criticBuilder = new FakeCriticBuilder("critic-instructions");
        var factory = CreateFactory(writerBuilder, criticBuilder);

        var agent = factory.CreateCriticAgent("api-key", "gpt-4o-mini", 200);

        Assert.Equal("Critic", agent.Name);
        Assert.Equal(1, criticBuilder.BuildInstructionsCalls);
        Assert.Equal(200, criticBuilder.LastFeedbackMaxLength);
        Assert.Equal(0, writerBuilder.BuildInstructionsCalls);
    }

    [Fact]
    public void ResolveModelToUse_ShouldReturnOverride_WhenModelIsAllowed()
    {
        var factory = CreateFactory(
            new FakeWriterBuilder("writer-instructions"),
            new FakeCriticBuilder("critic-instructions"));

        var model = factory.ResolveModelToUse("gpt-4.1-mini");

        Assert.Equal("gpt-4.1-mini", model);
    }

    [Fact]
    public void ResolveModelToUse_ShouldAcceptOverrideWithDifferentCaseAndWhitespace_AndReturnCanonicalModel()
    {
        var factory = CreateFactory(
            new FakeWriterBuilder("writer-instructions"),
            new FakeCriticBuilder("critic-instructions"));

        var model = factory.ResolveModelToUse(" GPT-4O-MINI ");

        Assert.Equal("gpt-4o-mini", model);
    }

    [Theory]
    [InlineData("gpt-not-allowed")]
    [InlineData("    ")]
    [InlineData("")]
    public void ResolveModelToUse_ShouldFallbackToDefault_WhenOverrideIsNotAllowed(string modelOverride)
    {
        var factory = CreateFactory(
            new FakeWriterBuilder("writer-instructions"),
            new FakeCriticBuilder("critic-instructions"));

        var model = factory.ResolveModelToUse(modelOverride);

        Assert.Equal("gpt-5-nano", model);
    }

    [Fact]
    public void ResolveModelToUse_ShouldAllowDefault_WhenAvailableModelsIsEmpty()
    {
        var factory = CreateFactory(
            new FakeWriterBuilder("writer-instructions"),
            new FakeCriticBuilder("critic-instructions"),
            "gpt-4.1-nano",
            []);

        var model = factory.ResolveModelToUse("gpt-4.1-nano");

        Assert.Equal("gpt-4.1-nano", model);
    }

    private static WriterCriticAgentFactory CreateFactory(
        FakeWriterBuilder writerBuilder,
        FakeCriticBuilder criticBuilder,
        string defaultModel = "gpt-5-nano",
        List<string>? availableModels = null)
    {
        var options = Options.Create(new AgentFrameworkOptions
        {
            DefaultModel = defaultModel,
            AvailableModels = availableModels ?? ["gpt-5-nano", "gpt-4.1-mini", "gpt-4o-mini"]
        });

        return new WriterCriticAgentFactory(writerBuilder, criticBuilder, options);
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
