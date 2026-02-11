using PetWorld.Domain.Base;

namespace PetWorld.Domain.ValueObjects;

public sealed record IterationCount
{
    private const int MinValue = 1;
    public const int MaxValue = 3;

    private IterationCount(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static Result<IterationCount> Create(int value)
    {
        return value is < MinValue or > MaxValue ? Result.Fail<IterationCount>($"Iteration count must be between {MinValue} and {MaxValue}.") : Result.Success(new IterationCount(value));
    }
}