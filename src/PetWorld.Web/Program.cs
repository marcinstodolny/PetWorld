using PetWorld.Application;
using PetWorld.Application.Configuration;
using PetWorld.Infrastructure;
using PetWorld.Web.Components;

namespace PetWorld.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
            builder.Services
                .AddOptions<AgentFrameworkOptions>()
                .BindConfiguration(AgentFrameworkOptions.SectionName)
                .PostConfigure(options =>
                {
                    if (options.AvailableModels.Count == 0)
                    {
                        options.AvailableModels.Add(options.DefaultModel);
                    }

                })
                .Validate(options => !string.IsNullOrWhiteSpace(options.DefaultModel), "DefaultModel is required.")
                .Validate(options => options.AvailableModels is { Count: > 0 }, "AvailableModels must contain at least one model.")
                .Validate(options => options.AvailableModels.Contains(options.DefaultModel, StringComparer.OrdinalIgnoreCase), "DefaultModel must be included in AvailableModels.")
                .ValidateOnStart();
            builder.Services.AddApplication();
            builder.Services.AddInfrastructure(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
            app.UseHttpsRedirection();

            app.UseAntiforgery();

            if (!app.Environment.IsEnvironment("Testing"))
            {
                app.MapStaticAssets();
            }
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Services.ApplyInfrastructureMigrations();

            app.Run();
        }
    }
}
