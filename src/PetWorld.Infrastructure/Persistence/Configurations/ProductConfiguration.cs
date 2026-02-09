using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetWorld.Domain.Entities;
using PetWorld.Domain.ValueObjects;

namespace PetWorld.Infrastructure.Persistence.Configurations;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");
        builder.HasKey(product => product.Id);

        builder.Property(product => product.Name)
            .HasConversion(name => name.Value, value => ProductName.Create(value).Value)
            .HasMaxLength(ProductName.MaxLength)
            .IsRequired();

        builder.Property(product => product.Category)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(product => product.Description)
            .HasConversion(description => description.Value, value => ProductDescription.Create(value).Value)
            .HasMaxLength(ProductDescription.MaxLength)
            .IsRequired();

        builder.Property(product => product.CreatedAt)
            .IsRequired();

        builder.Property(product => product.UpdatedAt);

        builder.HasData(ProductSeedData.Products);

        builder.OwnsOne(product => product.Price, price =>
        {
            price.Property(value => value.Amount)
                .HasColumnName("PriceAmount")
                .HasPrecision(10, 2)
                .IsRequired();

            price.Property(value => value.Currency)
                .HasColumnName("PriceCurrency")
                .HasMaxLength(3)
                .IsRequired();

            price.HasData(ProductSeedData.Prices);
        });

    }
}
