using PetWorld.Domain.Entities;

namespace PetWorld.Application.Abstraction.Repository;

public interface IChatMessageRepository
{
    Task AddAsync(ChatMessage message, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ChatMessage>> GetHistoryAsync(CancellationToken cancellationToken = default);
}
