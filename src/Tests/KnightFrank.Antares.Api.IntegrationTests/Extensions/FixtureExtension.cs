namespace KnightFrank.Antares.Api.IntegrationTests.Extensions
{
    using System.Net.Http;

    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;

    public static class FixtureExtension
    {
        public static HttpResponseMessage SendGetRequest(this BaseTestClassFixture fixture, string request)
        {
            return fixture.Server.HttpClient.GetAsync(request).Result;
        }

        public static HttpResponseMessage SendPostRequest(this BaseTestClassFixture fixture, string request, object content)
        {
            return fixture.Server.HttpClient.PostAsJsonAsync(request, content).Result;
        }

        public static HttpResponseMessage SendPutRequest(this BaseTestClassFixture fixture, string request, object content)
        {
            return fixture.Server.HttpClient.PutAsJsonAsync(request, content).Result;
        }
    }
}
