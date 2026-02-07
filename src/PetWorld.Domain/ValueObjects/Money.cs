using PetWorld.Domain.Base;

namespace PetWorld.Domain.ValueObjects;

public sealed record Money
{
    private Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public decimal Amount { get; }
    public string Currency { get; }

    public static Result<Money> Create(decimal amount, string currency)
    {
        if (amount <= 0)
        {
            return Result.Fail<Money>("Price amount must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(currency))
        {
            return Result.Fail<Money>("Currency cannot be empty.");
        }

        return Result.Success(new Money(amount, currency.Trim().ToUpperInvariant()));
    }
}