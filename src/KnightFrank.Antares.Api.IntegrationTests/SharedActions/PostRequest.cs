using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightFrank.Antares.Api.IntegrationTests.SharedActions
{

    using System.Net.Http;

    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Dal.Model;

    using TechTalk.SpecFlow;

    public static class PostRequest
    {
        public static void SendPostRequest(BaseTestClassFixture fixture, string request, object content)
        {
            HttpResponseMessage response = fixture.Server.HttpClient.PostAsJsonAsync(request, content).Result;
            ScenarioContext.Current.Set(response, "Response");
        }
    }
}
