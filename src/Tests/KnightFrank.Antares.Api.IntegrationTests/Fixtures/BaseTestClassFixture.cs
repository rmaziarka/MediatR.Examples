namespace KnightFrank.Antares.Api.IntegrationTests.Fixtures
{
    using System;
    using System.Data.Entity;

    using KnightFrank.Antares.Api.IntegrationTests.MockExternalServices;
    using KnightFrank.Antares.Api.Services.AzureStorage;
    using KnightFrank.Antares.Dal;

    using Microsoft.Owin.Testing;

    // ReSharper disable once ClassNeverInstantiated.Global
    public class BaseTestClassFixture : IDisposable
    {
        private readonly DbContextTransaction transaction;

        public BaseTestClassFixture()
        {
            this.DataContext = new KnightFrankContext();

            this.transaction = this.DataContext.Database.BeginTransaction();

            NinjectWebCommon.RebindAction =
                kernel =>
                {
                    kernel.Rebind<KnightFrankContext>().ToMethod(context => this.DataContext);
                    kernel.Rebind<IStorageClientWrapper>().To<MockStorageClient>();
                };
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
