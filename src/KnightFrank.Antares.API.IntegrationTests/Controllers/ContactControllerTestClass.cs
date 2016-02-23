using System.Net;

using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
using Xunit;

namespace KnightFrank.Antares.Api.IntegrationTests.Controllers
{
    using System.Net.Http;

    public class ContactControllerTestClass : IClassFixture<BaseTestClassFixture>
    {
        private readonly BaseTestClassFixture fixture;

        private const string ApiUrl = "/api/contact";

        public ContactControllerTestClass(BaseTestClassFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async void GetContact_ShouldReturnValidResult()
        {
            string requestUrl = $"{ApiUrl}/1";
            HttpResponseMessage response = await this.fixture.Server.HttpClient.GetAsync(requestUrl);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
