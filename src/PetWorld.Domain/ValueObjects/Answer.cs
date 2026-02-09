using PetWorld.Domain.Base;

namespace PetWorld.Domain.ValueObjects;

public sealed record Answer
{
    public const int MaxLength = 4000;

    private Answer(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Answer> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Fail<Answer>("Answer cannot be empty.");
        }

        var trimmed = value.Trim();
        if (trimmed.Length > MaxLength)
        {
            return Result.Fail<Answer>($"Answer cannot exceed {MaxLength} characters.");
        }

        return Result.Success(new Answer(trimmed));
    }
}