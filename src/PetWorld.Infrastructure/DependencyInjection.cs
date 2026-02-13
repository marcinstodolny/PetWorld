using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetWorld.Application.Abstraction.AI;
using PetWorld.Application.Abstraction.Repository;
using PetWorld.Infrastructure.Persistence;
using PetWorld.Infrastructure.Repositories;
using PetWorld.Infrastructure.Services;

namespace PetWorld.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PetWorldDatabase");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Connection string 'PetWorldDatabase' is missing.");
        }

        services.AddDbContext<PetWorldDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), mySqlOptions =>
                mySqlOptions.EnableRetryOnFailure()));

        services.Configure<AgentFrameworkOptions>(configuration.GetSection(AgentFrameworkOptions.SectionName));

        services.AddScoped<IChatMessageRepository, ChatMessageRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IWriterCriticService, WriterCriticService>();

        return services;
    }
}
