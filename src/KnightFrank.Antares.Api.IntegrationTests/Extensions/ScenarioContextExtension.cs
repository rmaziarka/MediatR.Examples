namespace KnightFrank.Antares.Api.IntegrationTests.Extensions
{
    using System.Net;
    using System.Net.Http;

    using TechTalk.SpecFlow;

    public static class ScenarioContextExtension
    {
        public static string GetResponseContent(this ScenarioContext scenarioContext)
        {
            return scenarioContext.GetHttpResponseMessage().Content.ReadAsStringAsync().Result;
        }

        public static T GetResponse<T>(this ScenarioContext scenarioContext)
        {
            return scenarioContext.GetHttpResponseMessage().Content.ReadAsAsync<T>().Result;
        }

        public static HttpStatusCode GetResponseHttpStatusCode(this ScenarioContext scenarioContext)
        {
            return scenarioContext.GetHttpResponseMessage().StatusCode;
        }

        public static ScenarioContext SetHttpResponseMessage(this ScenarioContext scenarioContext,
            HttpResponseMessage responseMessage)
        {
            scenarioContext.Set(responseMessage, "Response");
            return scenarioContext;
        }

        private static HttpResponseMessage GetHttpResponseMessage(this SpecFlowContext scenarioContext)
        {
            return scenarioContext.Get<HttpResponseMessage>("Response");
        }
    }
}
