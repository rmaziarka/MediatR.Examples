namespace KnightFrank.Antares.Api.IntegrationTests.Steps.Property
{
    using System;
    using System.Net.Http;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;

    using TechTalk.SpecFlow;

    [Binding]
    public class PropertyTypesSteps
    {
        private const string ApiUrl = "/api/properties/types";
        private readonly BaseTestClassFixture fixture;

        private readonly ScenarioContext scenarioContext;

        public PropertyTypesSteps(BaseTestClassFixture fixture, ScenarioContext scenarioContext)
        {
            this.fixture = fixture;
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            this.scenarioContext = scenarioContext;
        }

        [When(@"User gets property types for (.*) division and (.*) country")]
        public void GivenUserGetsPropertyTypesForDivisionAndCountry(string divisionCode, string countryCode)
        {
            string requestUrl = $"{ApiUrl}?countryCode={countryCode}&divisionCode={divisionCode}";
            HttpResponseMessage response = this.fixture.SendGetRequest(requestUrl);
            this.scenarioContext.SetHttpResponseMessage(response);
        }
    }
}
