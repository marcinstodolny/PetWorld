using PetWorld.Domain.Base;

namespace PetWorld.Domain.ValueObjects;

public sealed record ProductDescription
{
    public const int MaxLength = 1000;

    private ProductDescription(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<ProductDescription> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Fail<ProductDescription>("Product description cannot be empty.");
        }

        var trimmed = value.Trim();
        if (trimmed.Length > MaxLength)
        {
            return Result.Fail<ProductDescription>($"Product description cannot exceed {MaxLength} characters.");
        }

        return Result.Success(new ProductDescription(trimmed));
    }
}