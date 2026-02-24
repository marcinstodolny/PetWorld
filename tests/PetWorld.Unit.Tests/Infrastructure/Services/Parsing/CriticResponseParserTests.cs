using Microsoft.Extensions.Logging.Abstractions;
using PetWorld.Infrastructure.Services.WriterCritic.Parsing;

namespace PetWorld.Unit.Tests.Infrastructure.Services.Parsing;

public class CriticResponseParserTests
{
    private const string DefaultFeedback = "Domyślny feedback";

    [Fact]
    public void Parse_WhenValidJson_ShouldReturnParsedValues()
    {
        var parser = new CriticResponseParser(NullLogger<CriticResponseParser>.Instance);

        var result = parser.Parse("{\"approved\":true,\"feedback\":\"Popraw cenę\"}", 200, DefaultFeedback);

        Assert.True(result.Approved);
        Assert.Equal("Popraw cenę", result.Feedback);
    }

    [Fact]
    public void Parse_WhenJsonWrappedInExtraBraces_ShouldExtractJson()
    {
        var parser = new CriticResponseParser(NullLogger<CriticResponseParser>.Instance);

        var result = parser.Parse("{{\"approved\":false,\"feedback\":\"Skróć uzasadnienie\"}}", 200, DefaultFeedback);

        Assert.False(result.Approved);
        Assert.Equal("Skróć uzasadnienie", result.Feedback);
    }

    [Fact]
    public void Parse_WhenInvalidResponse_ShouldReturnDefaultFeedback()
    {
        var parser = new CriticResponseParser(NullLogger<CriticResponseParser>.Instance);

        var result = parser.Parse("brak json", 200, DefaultFeedback);

        Assert.False(result.Approved);
        Assert.Equal(DefaultFeedback, result.Feedback);
    }
}