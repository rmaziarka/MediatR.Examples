using System;

using Microsoft.Owin.Testing;

namespace KnightFrank.Antares.Api.IntegrationTests.Fixtures
{
    public class BaseTestClassFixture : IDisposable
    {
        public TestServer Server { get; }

        public BaseTestClassFixture()
        {
            this.Server = TestServer.Create<Startup>();
        }

        public void Dispose()
        {
            this.Server.Dispose();
        }
    }
}
