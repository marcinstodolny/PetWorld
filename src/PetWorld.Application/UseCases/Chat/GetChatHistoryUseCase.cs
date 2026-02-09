using PetWorld.Application.Abstraction.Repository;
using PetWorld.Application.Models.Response;

namespace PetWorld.Application.UseCases.Chat;

public sealed class GetChatHistoryUseCase(IChatMessageRepository chatMessageRepository)
{
    public async Task<IReadOnlyList<ChatHistoryItemResponse>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var messages = await chatMessageRepository.GetHistoryAsync(cancellationToken);

        return messages
            .Select(message => new ChatHistoryItemResponse(
                message.CreatedAt,
                message.Question.Value,
                message.Answer.Value,
                message.IterationCount.Value))
            .ToList();
    }
}
