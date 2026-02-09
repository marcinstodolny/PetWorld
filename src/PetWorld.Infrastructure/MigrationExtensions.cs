using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetWorld.Infrastructure.Persistence;

namespace PetWorld.Infrastructure;

public static class MigrationExtensions
{
    public static void ApplyInfrastructureMigrations(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PetWorldDbContext>();
        dbContext.Database.Migrate();
    }
}
