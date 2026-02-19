namespace PetWorld.Integration.Tests.Infrastructure;

public sealed class IntegrationTestFixture : IAsyncLifetime
{
    private readonly MySqlTestContainerFixture _databaseFixture = new();

    public CustomWebApplicationFactory? Factory { get; private set; }
    public DatabaseRespawner? Respawner { get; private set; }
    public bool IsDockerAvailable => _databaseFixture.IsDockerAvailable;
    public string DockerUnavailableReason => _databaseFixture.DockerUnavailableReason ?? "Docker is unavailable.";

    public HttpClient CreateClient()
    {
        return Factory is null ? throw new InvalidOperationException("Test factory is not initialized because Docker is unavailable.") : Factory.CreateClient();
    }

    public async Task InitializeAsync()
    {
        await _databaseFixture.InitializeAsync();
        if (!IsDockerAvailable)
        {
            return;
        }

        Factory = new CustomWebApplicationFactory(_databaseFixture.ConnectionString);
        Respawner = new DatabaseRespawner(_databaseFixture.ConnectionString);
    }

    public async Task DisposeAsync()
    {
        Factory?.Dispose();
        await _databaseFixture.DisposeAsync();
    }
}
