using PetWorld.Domain.Base;

namespace PetWorld.Domain.ValueObjects;

public sealed record Question
{
    public const int MaxLength = 2000;

    private Question(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Question> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Fail<Question>("Question cannot be empty.");
        }

        var trimmed = value.Trim();
        if (trimmed.Length > MaxLength)
        {
            return Result.Fail<Question>($"Question cannot exceed {MaxLength} characters.");
        }

        return Result.Success(new Question(trimmed));
    }
}