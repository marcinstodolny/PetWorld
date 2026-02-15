using Microsoft.Extensions.Logging;
using PetWorld.Application.Abstraction.Repository;
using PetWorld.Application.Models.Response;
using PetWorld.Domain.Base;

namespace PetWorld.Application.UseCases.Chat;

public sealed class GetChatHistoryUseCase(
    IChatMessageRepository chatMessageRepository,
    ILogger<GetChatHistoryUseCase> logger)
{
    public async Task<Result<IReadOnlyList<ChatHistoryItemResponse>>> ExecuteAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            var messages = await chatMessageRepository.GetHistoryAsync(cancellationToken);

            var history = messages
                .Select(message => new ChatHistoryItemResponse(
                    message.CreatedAt,
                    message.Question.Value,
                    message.Answer.Value,
                    message.IterationCount.Value))
                .ToList();

            return Result.Success<IReadOnlyList<ChatHistoryItemResponse>>(history);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Failed to load chat history.");
            return Result.Fail<IReadOnlyList<ChatHistoryItemResponse>>("Nie udało się pobrać historii czatu.");
        }
    }
}