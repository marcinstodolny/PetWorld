using PetWorld.Infrastructure.Services.WriterCritic.Prompts;

namespace PetWorld.Unit.Tests.Infrastructure.Services.Prompts;

public class WriterBuilderTests
{
    [Fact]
    public void Build_WhenFeedbackProvided_ShouldIncludeFeedbackSection()
    {
        var builder = new WriterBuilder();

        var prompt = builder.BuildPrompt("Jaką karmę wybrać?", "1) Karma", "Skróć uzasadnienia.");

        Assert.Contains("=== FEEDBACK KRYTYKA", prompt);
        Assert.Contains("Skróć uzasadnienia.", prompt);
        Assert.Contains("=== KONIEC DANYCH ===", prompt);
    }

    [Fact]
    public void Build_WhenFeedbackMissing_ShouldNotIncludeFeedbackSection()
    {
        var builder = new WriterBuilder();

        var prompt = builder.BuildPrompt("Jaką karmę wybrać?", "1) Karma", null);

        Assert.DoesNotContain("=== FEEDBACK KRYTYKA", prompt);
        Assert.Contains("=== KATALOG PRODUKTÓW", prompt);
    }
}
