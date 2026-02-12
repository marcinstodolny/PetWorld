using PetWorld.Domain.ValueObjects;

namespace PetWorld.Unit.Tests.ValueObjects;

public class AnswerTests
{
    [Theory]
    [InlineData("Valid answer")]
    [InlineData("  Trimmed answer  ")]
    public void Create_Valid_ShouldSuccess(string value)
    {
        var result = Answer.Create(value);

        Assert.True(result.IsSuccess);
        Assert.Equal(value.Trim(), result.Value.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_Empty_ShouldFail(string? value)
    {
        var result = Answer.Create(value!);

        Assert.True(result.IsFailed);
    }

    [Fact]
    public void Create_TooLong_ShouldFail()
    {
        var value = new string('a', Answer.MaxLength + 1);

        var result = Answer.Create(value);

        Assert.True(result.IsFailed);
    }
}
