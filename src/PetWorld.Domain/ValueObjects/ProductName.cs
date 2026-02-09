using PetWorld.Domain.Base;

namespace PetWorld.Domain.ValueObjects;

public sealed record ProductName
{
    public const int MaxLength = 200;

    private ProductName(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<ProductName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Fail<ProductName>("Product name cannot be empty.");
        }

        var trimmed = value.Trim();
        if (trimmed.Length > MaxLength)
        {
            return Result.Fail<ProductName>($"Product name cannot exceed {MaxLength} characters.");
        }

        return Result.Success(new ProductName(trimmed));
    }
}