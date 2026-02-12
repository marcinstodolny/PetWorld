using PetWorld.Domain.ValueObjects;

namespace PetWorld.Unit.Tests.ValueObjects;

public class ProductNameTests
{
    [Theory]
    [InlineData("Valid Name")]
    [InlineData("  Trimmed Name  ")]
    public void Create_Valid_ShouldSuccess(string name)
    {
        var result = ProductName.Create(name);

        Assert.True(result.IsSuccess);
        Assert.Equal(name.Trim(), result.Value.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_Empty_ShouldFail(string? name)
    {
        var result = ProductName.Create(name!);

        Assert.True(result.IsFailed);
    }

    [Fact]
    public void Create_TooLong_ShouldFail()
    {
        var name = new string('a', ProductName.MaxLength + 1);

        var result = ProductName.Create(name);

        Assert.True(result.IsFailed);
    }
}
