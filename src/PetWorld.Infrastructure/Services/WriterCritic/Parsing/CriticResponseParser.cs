using System.Text.Json;
using PetWorld.Domain.Base;

namespace PetWorld.Infrastructure.Services.WriterCritic.Parsing;

public sealed class CriticResponseParser : ICriticResponseParser
{
    private static readonly JsonSerializerOptions SJsonOptions = new() { PropertyNameCaseInsensitive = true };

    public (bool Approved, string Feedback) Parse(string response, int feedbackMaxLength, string defaultFeedback)
    {
        try
        {
            var json = ExtractJson(response);
            if (json.IsFailed)
            {
                return (false, defaultFeedback);
            }

            var parsed = JsonSerializer.Deserialize<CriticResponse>(json.Value, SJsonOptions);
            if (parsed is not null)
            {
                var feedback = NormalizeFeedback(parsed.Feedback, feedbackMaxLength, defaultFeedback);
                return (parsed.Approved, feedback);
            }
        }
        catch (JsonException)
        {
            // ignore
        }

        return (false, defaultFeedback);
    }

    private static string NormalizeFeedback(string? feedback, int feedbackMaxLength, string defaultFeedback)
    {
        var normalized = string.IsNullOrWhiteSpace(feedback)
            ? defaultFeedback
            : feedback.Trim();

        return normalized.Length <= feedbackMaxLength
            ? normalized
            : normalized[..feedbackMaxLength];
    }

    private static Result<string> ExtractJson(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return Result<string>.Fail("Response is empty.");

        var trimmed = text.Trim();

        while (trimmed.StartsWith("{{") && trimmed.EndsWith("}}"))
            trimmed = trimmed[1..^1].Trim();

        var start = trimmed.IndexOf('{');
        var end = trimmed.LastIndexOf('}');
        if (start < 0 || end <= start)
            return Result<string>.Fail("Response does not contain valid JSON.");

        return Result.Success(trimmed.Substring(start, end - start + 1));
    }

    private sealed record CriticResponse(bool Approved, string? Feedback);
}