using Microsoft.Extensions.DependencyInjection;
using PetWorld.Application.UseCases.Chat;

namespace PetWorld.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<AskChatUseCase>();
        services.AddScoped<GetChatHistoryUseCase>();

        return services;
    }
}
