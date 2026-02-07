using PetWorld.Domain.Abstraction;
using PetWorld.Domain.Enums;
using PetWorld.Domain.ValueObjects;

namespace PetWorld.Domain.Entities;

public sealed class Product : Entity<Guid>
{
    private Product() { }

    private Product(Guid id, ProductName name, ProductCategory category, Money price, ProductDescription description)
        : base(id)
    {
        Name = name;
        Category = category;
        Price = price;
        Description = description;
    }

    public ProductName Name { get; private set; } = null!;
    public ProductCategory Category { get; private set; }
    public Money Price { get; private set; } = null!;
    public ProductDescription Description { get; private set; } = null!;

    public static Product Create(ProductName name, ProductCategory category, Money price, ProductDescription description)
    {
        return new Product(Guid.NewGuid(), name, category, price, description);
    }

    public void Update(ProductName name, ProductCategory category, Money price, ProductDescription description)
    {
        Name = name;
        Category = category;
        Price = price;
        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }
}