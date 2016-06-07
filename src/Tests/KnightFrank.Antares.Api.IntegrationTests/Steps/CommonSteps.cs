namespace KnightFrank.Antares.Api.IntegrationTests.Steps
{
    using System;
    using System.Net;

    using FluentAssertions;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;

    using TechTalk.SpecFlow;

    using Xunit;

    [Binding]
    public class CommonSteps : IClassFixture<BaseTestClassFixture>
    {
        private readonly ScenarioContext scenarioContext;

        public CommonSteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            this.scenarioContext = scenarioContext;
        }

        [Then(@"User should get (.*) http status code")]
        public void ThenStatusCodeShouldBe(HttpStatusCode statusCode)
        {
            var abc = this.scenarioContext.GetResponseContent();
            this.scenarioContext.GetResponseHttpStatusCode().Should().Be(statusCode);
        }
    }
}
