using Microsoft.EntityFrameworkCore;
using PetWorld.Application.Abstraction.Repository;
using PetWorld.Domain.Entities;
using PetWorld.Infrastructure.Persistence;

namespace PetWorld.Infrastructure.Repositories;

public sealed class EfChatMessageRepository(PetWorldDbContext dbContext) : IChatMessageRepository
{
    public async Task AddAsync(ChatMessage message, CancellationToken cancellationToken = default)
    {
        await dbContext.ChatMessages.AddAsync(message, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ChatMessage>> GetHistoryAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.ChatMessages
            .AsNoTracking()
            .OrderByDescending(message => message.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
