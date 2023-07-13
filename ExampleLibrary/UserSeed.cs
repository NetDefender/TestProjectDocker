using Dapper;
using Microsoft.Data.SqlClient;

namespace TestProjectDocker.ExampleLibrary;

public class UserSeed
{
    private readonly string _connectionString;
    private static readonly User[] _users = new[]
    {
        new User(1, "Miriam", DateTime.UtcNow),
        new User(2, "Luis", DateTime.UtcNow.AddDays(1)),
        new User(3, "Daniel", DateTime.UtcNow.AddDays(2)),
        new User(4, "Oscar", DateTime.UtcNow.AddDays(3)),
        new User(5, "Delia", DateTime.UtcNow.AddDays(4))
    };

    public UserSeed(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<int> InitializeAsync()
    {
        using SqlConnection connection = new (_connectionString);
        const string DROP = "DROP TABLE IF EXISTS [User]";
        const string CREATE = "CREATE TABLE [User](Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY, Name VARCHAR(200) NOT NULL, Created DATETIME NOT NULL DEFAULT GETDATE())";
        const string BULK = "INSERT [User] (Name) VALUES(@Name)";
        await connection.ExecuteAsync(DROP);
        await connection.ExecuteAsync(CREATE);
        return await connection.ExecuteAsync(sql: BULK, param: _users);
    }

    public async Task<int> UpdateCreatedAsync(DateTime when)
    {
        using SqlConnection connection = new(_connectionString);
        const string UPDATE = "UPDATE [User] SET Created = @When";
        const string COUNT = "SELECT COUNT(1) FROM [User] WHERE Created = @When";
        await connection.ExecuteAsync(sql: UPDATE, param: new { When = when });
        return await connection.ExecuteScalarAsync<int>(sql: COUNT, param: new { When = when });
    }
}
