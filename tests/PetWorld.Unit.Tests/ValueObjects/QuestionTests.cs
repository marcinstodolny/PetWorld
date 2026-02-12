using PetWorld.Domain.ValueObjects;

namespace PetWorld.Unit.Tests.ValueObjects;

public class QuestionTests
{
    [Theory]
    [InlineData("Valid question")]
    [InlineData("  Trimmed question  ")]
    public void Create_Valid_ShouldSuccess(string value)
    {
        var result = Question.Create(value);

        Assert.True(result.IsSuccess);
        Assert.Equal(value.Trim(), result.Value.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_Empty_ShouldFail(string? value)
    {
        var result = Question.Create(value!);

        Assert.True(result.IsFailed);
    }

    [Fact]
    public void Create_TooLong_ShouldFail()
    {
        var value = new string('a', Question.MaxLength + 1);

        var result = Question.Create(value);

        Assert.True(result.IsFailed);
    }
}
