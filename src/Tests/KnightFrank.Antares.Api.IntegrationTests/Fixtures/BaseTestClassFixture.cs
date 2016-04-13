namespace KnightFrank.Antares.Api.IntegrationTests.Fixtures
{
    using System;
    using System.Data.Entity;

    using KnightFrank.Antares.Dal;

    using Microsoft.Owin.Testing;

    // ReSharper disable once ClassNeverInstantiated.Global
    public class BaseTestClassFixture : IDisposable
    {
        private readonly DbContextTransaction transaction;

        public BaseTestClassFixture()
        {
            Database.SetInitializer(new DropCreateSeedDatabaseAlwaysInitializer());
            this.DataContext = new KnightFrankContext();
            this.DataContext.Database.Initialize(false);

            this.transaction = this.DataContext.Database.BeginTransaction();

            NinjectWebCommon.RebindAction =
                kernel => { kernel.Rebind<KnightFrankContext>().ToMethod(context => this.DataContext); };
            this.Server = TestServer.Create<Startup>();
        }

        public KnightFrankContext DataContext { get; }
        public TestServer Server { get; }

        public void Dispose()
        {
            this.transaction.Rollback();
            this.transaction.Dispose();
            this.DataContext.Dispose();
            this.Server.Dispose();
        }
    }
}
