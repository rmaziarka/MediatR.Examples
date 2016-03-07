namespace KnightFrank.Antares.Api.IntegrationTests.Fixtures
{
    using Xunit;

    [CollectionDefinition("Database collection")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {
    }
}
