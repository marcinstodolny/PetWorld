using PetWorld.Application.Abstraction.Repository;

namespace PetWorld.Infrastructure.Persistence;

public sealed class UnitOfWork(PetWorldDbContext dbContext) : IUnitOfWork
{
    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
