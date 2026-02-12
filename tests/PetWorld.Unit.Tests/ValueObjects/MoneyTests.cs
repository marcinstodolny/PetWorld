using PetWorld.Domain.ValueObjects;

namespace PetWorld.Unit.Tests.ValueObjects;

public class MoneyTests
{
    [Fact]
    public void Create_Valid_ShouldSuccess()
    {
        var result = Money.Create(10.5m, " pln ");

        Assert.True(result.IsSuccess);
        Assert.Equal(10.5m, result.Value.Amount);
        Assert.Equal("PLN", result.Value.Currency);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_NonPositiveAmount_ShouldFail(decimal amount)
    {
        var result = Money.Create(amount, "PLN");

        Assert.True(result.IsFailed);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_EmptyCurrency_ShouldFail(string? currency)
    {
        var result = Money.Create(10m, currency!);

        Assert.True(result.IsFailed);
    }
}
