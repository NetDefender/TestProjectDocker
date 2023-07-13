using Testcontainers.MsSql;

namespace TestProjectDocker;

/// <summary>
/// Shared state
/// </summary>
public sealed class ConnectionFixture : IAsyncLifetime
{
    private string _connectionString = default!;
    private readonly MsSqlContainer _container;
    public string ConnectionString => _connectionString;

    public ConnectionFixture()
    {
        _container = new MsSqlBuilder()
          .Build();
    }
    public async Task InitializeAsync()
    {
        await _container.StartAsync().ConfigureAwait(false);
        _connectionString = _container.GetConnectionString();
    }

    public async Task DisposeAsync()
    {
        await _container.DisposeAsync().ConfigureAwait(false);
    }
}