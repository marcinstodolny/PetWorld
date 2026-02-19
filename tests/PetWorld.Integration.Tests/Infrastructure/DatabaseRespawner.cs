using MySqlConnector;
using Respawn;

namespace PetWorld.Integration.Tests.Infrastructure;

public sealed class DatabaseRespawner(string connectionString)
{
    private Respawner? _respawner;

    public async Task ResetAsync()
    {
        await using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();

        _respawner ??= await Respawner.CreateAsync(connection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.MySql,
            TablesToIgnore = ["__EFMigrationsHistory"]
        });

        await _respawner.ResetAsync(connection);
    }
}
