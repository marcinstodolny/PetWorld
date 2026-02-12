using PetWorld.Domain.Entities;
using PetWorld.Domain.ValueObjects;

namespace PetWorld.Unit.Tests.Entities;

public class ChatMessageTests
{
    [Fact]
    public void Create_ShouldSetAllFields()
    {
        var question = Question.Create("Jaką karmę wybrać?").Value;
        var answer = Answer.Create("Najlepiej premium dla wieku psa.").Value;
        var iteration = IterationCount.Create(1).Value;

        var message = ChatMessage.Create(question, answer, iteration);

        Assert.NotEqual(Guid.Empty, message.Id);
        Assert.Equal(question, message.Question);
        Assert.Equal(answer, message.Answer);
        Assert.Equal(iteration, message.IterationCount);
        Assert.Null(message.UpdatedAt);
    }

    [Fact]
    public void UpdateAnswer_ShouldChangeAnswerAndIterationAndSetUpdatedAt()
    {
        var message = ChatMessage.Create(
            Question.Create("Pytanie").Value,
            Answer.Create("Stara odpowiedź").Value,
            IterationCount.Create(1).Value);

        var newAnswer = Answer.Create("Nowa odpowiedź").Value;
        var newIteration = IterationCount.Create(2).Value;

        message.UpdateAnswer(newAnswer, newIteration);

        Assert.Equal(newAnswer, message.Answer);
        Assert.Equal(newIteration, message.IterationCount);
        Assert.NotNull(message.UpdatedAt);
    }
}
