using PetWorld.Domain.Entities;
using PetWorld.Domain.Enums;
using PetWorld.Domain.ValueObjects;

namespace PetWorld.Unit.Tests.Entities;

public class ProductTests
{
    [Fact]
    public void Create_ShouldSetAllFields()
    {
        var name = ProductName.Create("Karma premium").Value;
        var price = Money.Create(99.99m, "pln").Value;
        var description = ProductDescription.Create("Opis produktu").Value;

        var product = Product.Create(name, ProductCategory.DogFood, price, description);

        Assert.NotEqual(Guid.Empty, product.Id);
        Assert.Equal(name, product.Name);
        Assert.Equal(ProductCategory.DogFood, product.Category);
        Assert.Equal(price, product.Price);
        Assert.Equal(description, product.Description);
        Assert.Null(product.UpdatedAt);
    }

    [Fact]
    public void Update_ShouldChangeFieldsAndSetUpdatedAt()
    {
        var product = Product.Create(
            ProductName.Create("Stara nazwa").Value,
            ProductCategory.DogFood,
            Money.Create(10m, "PLN").Value,
            ProductDescription.Create("Stary opis").Value);

        var newName = ProductName.Create("Nowa nazwa").Value;
        var newPrice = Money.Create(20m, "EUR").Value;
        var newDescription = ProductDescription.Create("Nowy opis").Value;

        product.Update(newName, ProductCategory.CatFood, newPrice, newDescription);

        Assert.Equal(newName, product.Name);
        Assert.Equal(ProductCategory.CatFood, product.Category);
        Assert.Equal(newPrice, product.Price);
        Assert.Equal(newDescription, product.Description);
        Assert.NotNull(product.UpdatedAt);
    }
}
