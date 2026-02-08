using PetWorld.Domain.Enums;
using PetWorld.Domain.ValueObjects;

namespace PetWorld.Infrastructure.Persistence;

internal static class ProductSeedData
{
    private static readonly SeedItem[] Items =
    {
        new SeedItem(
            id: Guid.Parse("6bf43c6c-2c24-4d82-9c2f-05a1e7c49a06"),
            name: "Royal Canin Adult Dog 15kg",
            category: ProductCategory.KarmaDlaPsow,
            priceAmount: 289m,
            priceCurrency: "PLN",
            description: "Premium karma dla dorosłych psów średnich ras"
        ),
        new SeedItem(
            id: Guid.Parse("3a7c1ad6-6fb4-4f9a-9a2d-7c11df74e6ff"),
            name: "Whiskas Adult Kurczak 7kg",
            category: ProductCategory.KarmaDlaKotow,
            priceAmount: 129m,
            priceCurrency: "PLN",
            description: "Sucha karma dla dorosłych kotów z kurczakiem"
        ),
        new SeedItem(
            id: Guid.Parse("e3fa1b9b-3758-46f1-8f3a-5f0c9a2a0e9e"),
            name: "Tetra AquaSafe 500ml",
            category: ProductCategory.Akwarystyka,
            priceAmount: 45m,
            priceCurrency: "PLN",
            description: "Uzdatniacz wody do akwarium, neutralizuje chlor"
        ),
        new SeedItem(
            id: Guid.Parse("f2a39b45-1f77-4c0b-a4b1-8a6b5d2c9a18"),
            name: "Trixie Drapak XL 150cm",
            category: ProductCategory.AkcesoriaDlaKotow,
            priceAmount: 399m,
            priceCurrency: "PLN",
            description: "Wysoki drapak z platformami i domkiem"
        ),
        new SeedItem(
            id: Guid.Parse("4e2f0a6b-8c17-4c65-96f1-1d7f0b5a2c3e"),
            name: "Kong Classic Large",
            category: ProductCategory.ZabawkiDlaPsow,
            priceAmount: 69m,
            priceCurrency: "PLN",
            description: "Wytrzymała zabawka do napełniania smakołykami"
        ),
        new SeedItem(
            id: Guid.Parse("9b1f4d62-9f6e-4b0e-8f42-4b2c7c4e2e6a"),
            name: "Ferplast Klatka dla chomika",
            category: ProductCategory.Gryzonie,
            priceAmount: 189m,
            priceCurrency: "PLN",
            description: "Klatka 60x40cm z wyposażeniem"
        ),
        new SeedItem(
            id: Guid.Parse("1ac2d2c4-8e26-4d1f-9a0a-93db3f5e2b79"),
            name: "Flexi Smycz automatyczna 8m",
            category: ProductCategory.AkcesoriaDlaPsow,
            priceAmount: 119m,
            priceCurrency: "PLN",
            description: "Smycz zwijana dla psów do 50kg"
        ),
        new SeedItem(
            id: Guid.Parse("2b5a69fd-4d44-4c32-bf5d-7f6b2a5d7f31"),
            name: "Brit Premium Kitten 8kg",
            category: ProductCategory.KarmaDlaKotow,
            priceAmount: 159m,
            priceCurrency: "PLN",
            description: "Karma dla kociąt do 12 miesiąca życia"
        ),
        new SeedItem(
            id: Guid.Parse("4c6f2a1b-5e7a-45ad-8b2f-63e2c9b1a7a4"),
            name: "JBL ProFlora CO2 Set",
            category: ProductCategory.Akwarystyka,
            priceAmount: 549m,
            priceCurrency: "PLN",
            description: "Kompletny zestaw CO2 dla roślin akwariowych"
        ),
        new SeedItem(
            id: Guid.Parse("5ad1d7e0-6d9c-4c0c-bf9a-0d84e2b2f4ef"),
            name: "Vitapol Siano dla królików 1kg",
            category: ProductCategory.Gryzonie,
            priceAmount: 25m,
            priceCurrency: "PLN",
            description: "Naturalne siano łąkowe, podstawa diety"
        )
    };

    internal static object[] Products => Items
        .Select(object (item) => new
        {
            Id = item.Id,
            Name = ProductName.Create(item.Name).Value,
            Category = item.Category,
            Description = ProductDescription.Create(item.Description).Value,
            CreatedAt = new DateTime(2026, 2, 8, 0, 0, 0, DateTimeKind.Utc),
            UpdatedAt = (DateTime?)null
        })
        .ToArray();

    internal static object[] Prices => Items
        .Select(object (item) => new
        {
            ProductId = item.Id,
            Amount = item.PriceAmount,
            Currency = item.PriceCurrency
        })
        .ToArray();

    private sealed class SeedItem
    {
        public SeedItem(Guid id, string name, ProductCategory category, decimal priceAmount, string priceCurrency, string description)
        {
            Id = id;
            Name = name;
            Category = category;
            PriceAmount = priceAmount;
            PriceCurrency = priceCurrency;
            Description = description;
        }

        public Guid Id { get; }
        public string Name { get; }
        public ProductCategory Category { get; }
        public decimal PriceAmount { get; }
        public string PriceCurrency { get; }
        public string Description { get; }
    }
}