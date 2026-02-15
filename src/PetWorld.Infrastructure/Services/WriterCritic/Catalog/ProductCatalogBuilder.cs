using PetWorld.Domain.Entities;
using PetWorld.Domain.Enums;
using System.Text;

namespace PetWorld.Infrastructure.Services.WriterCritic.Catalog;

public sealed class ProductCatalogBuilder : IProductCatalogBuilder
{
    public string Build(IReadOnlyList<Product> products)
    {
        if (products.Count == 0)
        {
            return "Brak produktów w katalogu.";
        }

        var builder = new StringBuilder();
        var index = 1;

        foreach (var product in products)
        {
            builder.AppendLine(
                $"{index}) \"{product.Name.Value}\" | Kategoria: {product.Category.ToPolish()} | Cena: {product.Price.Amount} {product.Price.Currency} | Opis: {product.Description.Value}");
            index++;
        }

        return builder.ToString().Trim();
    }
}
