using PetWorld.Domain.Entities;
using PetWorld.Domain.Enums;
using PetWorld.Domain.ValueObjects;
using PetWorld.Infrastructure.Services.WriterCritic.Catalog;

namespace PetWorld.Unit.Tests.Infrastructure.Services.WriterCritic;

public class ProductCatalogBuilderTests
{
    [Fact]
    public void Build_WhenProductsEmpty_ShouldReturnFallbackMessage()
    {
        var builder = new ProductCatalogBuilder();

        var catalog = builder.Build([]);

        Assert.Equal("Brak produktów w katalogu.", catalog);
    }

    [Fact]
    public void Build_WhenProductsProvided_ShouldRenderIndexedCatalogRows()
    {
        var builder = new ProductCatalogBuilder();
        var products = new List<Product>
        {
            Product.Create(
                ProductName.Create("Karma Premium").Value,
                ProductCategory.DogFood,
                Money.Create(99.99m, "pln").Value,
                ProductDescription.Create("Pełnoporcjowa karma dla psa").Value),
            Product.Create(
                ProductName.Create("Żwirek Naturalny").Value,
                ProductCategory.CatAccessories,
                Money.Create(45m, "pln").Value,
                ProductDescription.Create("Zbrylający żwirek bentonitowy").Value)
        };

        var catalog = builder.Build(products);

        Assert.Contains("1) \"Karma Premium\"", catalog);
        Assert.Contains("Kategoria: Karma dla psów", catalog);
        Assert.True(
            catalog.Contains("Cena: 99,99 PLN") || catalog.Contains("Cena: 99.99 PLN"),
            "Catalog should include price with either comma or dot decimal separator.");
        Assert.Contains("2) \"Żwirek Naturalny\"", catalog);
    }
}
