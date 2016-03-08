namespace KnightFrank.Antares.Api.IntegrationTests
{
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    using TechTalk.SpecFlow;

    public static class ScenarioContextExtention
    {
        public static string GetResponseContent(this ScenarioContext scenarioContext)
        {
            var response = scenarioContext.Get<HttpResponseMessage>("Response");
            return response.Content.ReadAsStringAsync().Result;
        }

        public static HttpStatusCode GetResponseHttpStatusCode(this ScenarioContext scenarioContext)
        {
            var response = scenarioContext.Get<HttpResponseMessage>("Response");
            return response.StatusCode;
        }
    }
}
