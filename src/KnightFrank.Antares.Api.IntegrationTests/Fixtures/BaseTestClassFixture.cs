namespace KnightFrank.Antares.Api.IntegrationTests.Fixtures
{
    using System;

    using Microsoft.Owin.Testing;

    public class BaseTestClassFixture : IDisposable
    {
        public BaseTestClassFixture()
        {
            this.Server = TestServer.Create<Startup>();
        }

        public TestServer Server { get; }

        public void Dispose()
        {
            this.Server.Dispose();
        }
    }
}
