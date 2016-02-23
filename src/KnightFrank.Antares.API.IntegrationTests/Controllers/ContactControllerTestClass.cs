namespace KnightFrank.Antares.Api.IntegrationTests.Controllers
{
    using System.Net;
    using System.Net.Http;

    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;

    using Xunit;

    public class ContactControllerTestClass : IClassFixture<BaseTestClassFixture>
    {
        private const string ApiUrl = "/api/contact";

        private readonly BaseTestClassFixture fixture;

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
