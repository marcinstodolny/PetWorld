using PetWorld.Domain.ValueObjects;

namespace PetWorld.Unit.Tests.ValueObjects;

public class ProductDescriptionTests
{
    [Theory]
    [InlineData("Valid Description")]
    [InlineData("  Trimmed Description  ")]
    public void Create_Valid_ShouldSuccess(string description)
    {
        var result = ProductDescription.Create(description);

        Assert.True(result.IsSuccess);
        Assert.Equal(description.Trim(), result.Value.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_Empty_ShouldFail(string? description)
    {
        var result = ProductDescription.Create(description!);

        Assert.True(result.IsFailed);
    }

    [Fact]
    public void Create_TooLong_ShouldFail()
    {
        var description = new string('a', ProductDescription.MaxLength + 1);

        var result = ProductDescription.Create(description);

        Assert.True(result.IsFailed);
    }
}
