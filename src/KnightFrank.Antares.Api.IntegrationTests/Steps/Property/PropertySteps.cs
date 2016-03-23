namespace KnightFrank.Antares.Api.IntegrationTests.Steps.Property
{
    using System;

    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;

    using TechTalk.SpecFlow;

    [Binding]
    public class PropertySteps
    {
        private const string ApiUrl = "/api/property";
        private readonly BaseTestClassFixture fixture;

        private readonly ScenarioContext scenarioContext;

        public PropertySteps(BaseTestClassFixture fixture, ScenarioContext scenarioContext)
        {
            this.fixture = fixture;
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            this.scenarioContext = scenarioContext;
        }
    }
}
