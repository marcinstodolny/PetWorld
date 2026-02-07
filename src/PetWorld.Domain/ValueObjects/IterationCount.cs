using PetWorld.Domain.Base;

namespace PetWorld.Domain.ValueObjects;

public sealed record IterationCount
{
    private const int MinValue = 1;
    private const int MaxValue = 3;

    private IterationCount(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static Result<IterationCount> Create(int value)
    {
        if (value is < MinValue or > MaxValue)
        {
            return Result.Fail<IterationCount>("Iteration count must be between 1 and 3.");
        }

        return Result.Success(new IterationCount(value));
    }
}