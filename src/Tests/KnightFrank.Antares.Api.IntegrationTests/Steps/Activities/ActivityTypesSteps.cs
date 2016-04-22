namespace KnightFrank.Antares.Api.IntegrationTests.Steps.Activities
{
    using System;
    using System.Net.Http;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;

    using TechTalk.SpecFlow;

    [Binding]
    public class ActivityTypesSteps
    {
        private const string ApiUrl = "/api/activities/types";
        private readonly BaseTestClassFixture fixture;

        private readonly ScenarioContext scenarioContext;

        public ActivityTypesSteps(BaseTestClassFixture fixture, ScenarioContext scenarioContext)
        {
            this.fixture = fixture;
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            this.scenarioContext = scenarioContext;
        }

        [When(@"User gets activity types for property and (.*) country")]
        public void GetActivityTypesForProperty(string countryCode)
        {
            var propertyTypeId = this.scenarioContext.Get<Guid>("PropertyTypeId");
            string requestUrl = $"{ApiUrl}?countryCode={countryCode}&propertyTypeId={propertyTypeId}";

            HttpResponseMessage response = this.fixture.SendGetRequest(requestUrl);
            this.scenarioContext.SetHttpResponseMessage(response);
        }
    }
}
