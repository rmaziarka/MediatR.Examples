namespace KnightFrank.Antares.Api.IntegrationTests.SharedActions
{
    using System.Net.Http;

    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;

    using TechTalk.SpecFlow;

    public static class GetRequest
    {
        public static void SendGetRequest(BaseTestClassFixture fixture, string request)
        {
            HttpResponseMessage response = fixture.Server.HttpClient.GetAsync(request).Result;
            ScenarioContext.Current.Set(response, "Response");
        }
    }
}
