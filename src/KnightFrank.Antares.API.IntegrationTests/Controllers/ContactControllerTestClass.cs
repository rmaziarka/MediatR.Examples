using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
using Xunit;

namespace KnightFrank.Antares.Api.IntegrationTests.Controllers
{
    public class ContactControllerTestClass : IClassFixture<BaseTestClassFixture>
    {
        private readonly BaseTestClassFixture _fixture;

        private const string ApiUrl = "/api/contact";

        public ContactControllerTestClass(BaseTestClassFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async void GetContact_ShouldReturnValidResult()
        {
            var requestUrl = $"{ApiUrl}/1";
            var response = await _fixture.Server.HttpClient.GetAsync(requestUrl);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
