using Microsoft.EntityFrameworkCore;
using PetWorld.Application.Abstraction.Repository;
using PetWorld.Domain.Entities;
using PetWorld.Infrastructure.Persistence;

namespace PetWorld.Infrastructure.Repositories;

public sealed class ProductRepository(PetWorldDbContext dbContext) : IProductRepository
{
    public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Products
            .AsNoTracking()
            .OrderBy(product => product.Name.Value)
            .ToListAsync(cancellationToken);
    }
}
