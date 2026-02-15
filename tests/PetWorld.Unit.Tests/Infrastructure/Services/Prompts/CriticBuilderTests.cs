using PetWorld.Infrastructure.Services.WriterCritic.Prompts;

namespace PetWorld.Unit.Tests.Infrastructure.Services.Prompts;

public class CriticBuilderTests
{
    [Fact]
    public void Build_ShouldContainAllDataSectionsAndJsonContract()
    {
        var builder = new CriticBuilder();

        var prompt = builder.BuildPrompt("Pytanie", "Odpowiedź", "Katalog");

        Assert.Contains("=== PYTANIE KLIENTA (DANE) ===", prompt);
        Assert.Contains("=== ODPOWIEDŹ WRITER’A (DANE) ===", prompt);
        Assert.Contains("=== KATALOG (DANE / WERYFIKACJA) ===", prompt);
        Assert.Contains("{\"approved\":true|false,\"feedback\":\"...\"}", prompt);
    }
}
