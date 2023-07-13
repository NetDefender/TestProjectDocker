using System.Globalization;
using TestProjectDocker.ExampleLibrary;

namespace TestProjectDocker;

/// <summary>
/// Test with shared state for all Methods
/// </summary>
/// <seealso cref="Xunit.IClassFixture&lt;TestProjectDocker.ConnectionFixture&gt;" />
public sealed class UserTests : IClassFixture<ConnectionFixture>
{
    private readonly ConnectionFixture _connectionFixture;

    public UserTests(ConnectionFixture fixture)
    {
        _connectionFixture = fixture;
    }

    [Fact]
    public async Task UserSeed_Initialize_Should_Return_Five_Rows()
    {
        UserSeed seed = new (_connectionFixture.ConnectionString);
        int rows = await seed.InitializeAsync();
        Assert.Equal(5, rows);
    }

    [Theory]
    [InlineData("20250101")]
    [InlineData("20300101")]
    public async Task UserSeed_Update_Should_Return_Five_Rows(string whenText)
    {
        DateTime when = DateTime.ParseExact(whenText, "yyyyMMdd", CultureInfo.InvariantCulture);
        UserSeed seed = new(_connectionFixture.ConnectionString);
        await seed.InitializeAsync();
        int rows = await seed.UpdateCreatedAsync(when);
        Assert.Equal(5, rows);
    }
}