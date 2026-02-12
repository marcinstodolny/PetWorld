using PetWorld.Domain.ValueObjects;

namespace PetWorld.Unit.Tests.ValueObjects;

public class IterationCountTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void Create_Valid_ShouldSuccess(int value)
    {
        var result = IterationCount.Create(value);

        Assert.True(result.IsSuccess);
        Assert.Equal(value, result.Value.Value);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(4)]
    [InlineData(-1)]
    public void Create_OutOfRange_ShouldFail(int value)
    {
        var result = IterationCount.Create(value);

        Assert.True(result.IsFailed);
    }
}
