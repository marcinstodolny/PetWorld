using PetWorld.Domain.Entities;

namespace PetWorld.Application.Abstraction.Repository;

public interface IProductRepository
{
    Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default);
}
