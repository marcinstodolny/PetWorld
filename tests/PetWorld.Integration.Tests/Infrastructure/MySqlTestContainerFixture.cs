using DotNet.Testcontainers.Builders;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using PetWorld.Infrastructure.Persistence;
using Testcontainers.MySql;

namespace PetWorld.Integration.Tests.Infrastructure;

public sealed class MySqlTestContainerFixture : IAsyncLifetime
{
    private MySqlContainer? _container;

    public string ConnectionString => _container?.GetConnectionString() ?? string.Empty;
    public bool IsDockerAvailable { get; private set; } = true;
    public string? DockerUnavailableReason { get; private set; }

    public async Task InitializeAsync()
    {
        try
        {
            _container = new MySqlBuilder()
                .WithImage("mysql:8.4")
                .WithDatabase("petworld_integration_tests")
                .WithUsername("petworld")
                .WithPassword("petworld_pwd")
                .Build();

            await _container.StartAsync();
            await ApplyMigrationsAsync();
        }
        catch (DockerUnavailableException exception)
        {
            IsDockerAvailable = false;
            DockerUnavailableReason = exception.Message;
        }
    }

    public async Task DisposeAsync()
    {
        if (IsDockerAvailable && _container is not null)
        {
            await _container.DisposeAsync();
        }
    }

    private async Task ApplyMigrationsAsync()
    {
        var options = new DbContextOptionsBuilder<PetWorldDbContext>()
            .UseMySql(ConnectionString, await ServerVersion.AutoDetectAsync(ConnectionString))
            .Options;

        await using var context = new PetWorldDbContext(options);
        await context.Database.MigrateAsync();
    }

    public async Task<MySqlConnection> OpenConnectionAsync()
    {
        var connection = new MySqlConnection(ConnectionString);
        await connection.OpenAsync();
        return connection;
    }
}
