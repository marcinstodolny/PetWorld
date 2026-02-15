using PetWorld.Domain.Entities;

namespace PetWorld.Infrastructure.Services.WriterCritic.Catalog;

public interface IProductCatalogBuilder
{
    string Build(IReadOnlyList<Product> products);
}
