namespace KnightFrank.Antares.Api.IntegrationTests.Fixtures
{
    using System;
    using System.Data.Entity;

    using KnightFrank.Antares.Dal;

    public class DatabaseFixture : IDisposable
    {
        private readonly KnightFrankContext dataContext;

        public DatabaseFixture()
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<KnightFrankContext>());

            this.dataContext = new KnightFrankContext();
            this.dataContext.Database.Initialize(true);
        }

        public void Dispose()
        {
            this.dataContext.Dispose();
        }
    }
}
