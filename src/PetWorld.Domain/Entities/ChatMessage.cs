using PetWorld.Domain.Abstraction;
using PetWorld.Domain.ValueObjects;

namespace PetWorld.Domain.Entities;

public sealed class ChatMessage : Entity<Guid>
{
    private ChatMessage() { }

    private ChatMessage(Guid id, Question question, Answer answer, IterationCount iterationCount)
        : base(id)
    {
        Question = question;
        Answer = answer;
        IterationCount = iterationCount;
    }

    public Question Question { get; private set; } = null!;
    public Answer Answer { get; private set; } = null!;
    public IterationCount IterationCount { get; private set; } = null!;

    public static ChatMessage Create(Question question, Answer answer, IterationCount iterationCount)
    {
        return new ChatMessage(Guid.NewGuid(), question, answer, iterationCount);
    }

    public void UpdateAnswer(Answer answer, IterationCount iterationCount)
    {
        Answer = answer;
        IterationCount = iterationCount;
        UpdatedAt = DateTime.UtcNow;
    }
}