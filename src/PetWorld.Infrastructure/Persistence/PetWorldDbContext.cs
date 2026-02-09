using Microsoft.EntityFrameworkCore;
using PetWorld.Domain.Entities;
using PetWorld.Infrastructure.Persistence.Configurations;

namespace PetWorld.Infrastructure.Persistence;

public sealed class PetWorldDbContext(DbContextOptions<PetWorldDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new ChatMessageConfiguration());
    }
}
