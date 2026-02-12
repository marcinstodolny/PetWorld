using PetWorld.Domain.Base;

namespace PetWorld.Unit.Tests.Base;

public class ResultTests
{
    [Fact]
    public void Success_ShouldCreateSuccessfulResult()
    {
        var result = Result.Success();

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailed);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Fail_WithMessage_ShouldCreateFailedResult()
    {
        var result = Result.Fail("error");

        Assert.True(result.IsFailed);
        Assert.Contains("error", result.Errors);
    }

    [Fact]
    public void GenericSuccess_ShouldExposeValue()
    {
        var result = Result.Success("ok");

        Assert.True(result.IsSuccess);
        Assert.Equal("ok", result.Value);
    }

    [Fact]
    public void GenericFail_AccessingValue_ShouldThrow()
    {
        var result = Result.Fail<string>("error");

        Assert.True(result.IsFailed);
        Assert.Throws<InvalidOperationException>(() => _ = result.Value);
    }
}
