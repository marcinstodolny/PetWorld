using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetWorld.Application.Abstraction.AI;
using PetWorld.Application.Models.Request;
using PetWorld.Application.UseCases.Chat;
using PetWorld.Infrastructure.Persistence;
using PetWorld.Web;

namespace PetWorld.Integration.Tests.Infrastructure;

public sealed class CustomWebApplicationFactory(string connectionString) : WebApplicationFactory<Program>
{
    public FakeWriterCriticService FakeWriterCriticService { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            services.RemoveDbContextRegistration<PetWorldDbContext>();

            services.AddDbContext<PetWorldDbContext>(options =>
                options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 4, 0))));

            services.AddSingleton<IWriterCriticService>(FakeWriterCriticService);
        });

        builder.Configure(app =>
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapPost("/test-api/chat", async context =>
                {
                    var request = await context.Request.ReadFromJsonAsync<AskChatRequest>(context.RequestAborted);
                    if (request is null)
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsJsonAsync(new { error = "Request body is required." }, context.RequestAborted);
                        return;
                    }

                    var useCase = context.RequestServices.GetRequiredService<AskChatUseCase>();
                    var result = await useCase.ExecuteAsync(request, context.RequestAborted);
                    if (result.IsFailed)
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsJsonAsync(new { errors = result.Errors.ToArray() }, context.RequestAborted);
                        return;
                    }

                    await context.Response.WriteAsJsonAsync(result.Value, context.RequestAborted);
                });

                endpoints.MapGet("/test-api/chat/history", async context =>
                {
                    var useCase = context.RequestServices.GetRequiredService<GetChatHistoryUseCase>();
                    var result = await useCase.ExecuteAsync(context.RequestAborted);
                    if (result.IsFailed)
                    {
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        await context.Response.WriteAsJsonAsync(new { errors = result.Errors.ToArray() }, context.RequestAborted);
                        return;
                    }

                    await context.Response.WriteAsJsonAsync(result.Value, context.RequestAborted);
                });
            });
        });
    }
}

internal static class ServiceCollectionExtensions
{
    public static void RemoveDbContextRegistration<TDbContext>(this IServiceCollection services)
        where TDbContext : DbContext
    {
        var descriptors = services.Where(descriptor =>
            descriptor.ServiceType == typeof(DbContextOptions<TDbContext>) ||
            descriptor.ServiceType == typeof(TDbContext))
            .ToList();

        foreach (var descriptor in descriptors)
        {
            services.Remove(descriptor);
        }
    }
}
