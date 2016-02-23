using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Testing;

namespace KnightFrank.Antares.Api.IntegrationTests.Fixtures
{
    public class BaseTestClassFixture : IDisposable
    {
        public TestServer Server { get; }

        public BaseTestClassFixture()
        {
            Server = TestServer.Create<Startup>();
        }

        public void Dispose()
        {
            Server.Dispose();
        }
    }
}
