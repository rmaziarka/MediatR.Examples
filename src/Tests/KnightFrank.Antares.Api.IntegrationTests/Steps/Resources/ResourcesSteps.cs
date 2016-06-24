namespace KnightFrank.Antares.Api.IntegrationTests.Steps.Resources
{
    using System.Collections.Generic;
    using System.Net.Http;

    using FluentAssertions;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Domain.AddressForm.QueryResults;

    using Newtonsoft.Json;

    using TechTalk.SpecFlow;

    using Xunit;

    [Binding]
    public class ResourcesSteps : IClassFixture<BaseTestClassFixture>
    {
        private const string ApiUrl = "/api/resources";
        private readonly BaseTestClassFixture fixture;
        private readonly ScenarioContext scenarioContext;

        public ResourcesSteps(ScenarioContext scenarioContext, BaseTestClassFixture fixture)
        {
            this.scenarioContext = scenarioContext;
            this.fixture = fixture;
        }

        [When(@"User retrieves countries for (.*) EnumTypeItem")]
        public void WhenUserRetrievesCountriesForPropertyEnumTypeItem(string entityTypeItemCode)
        {
            string requestUrl = $"{ApiUrl}/countries/addressform?entityTypeItemCode={entityTypeItemCode}";
            HttpResponseMessage response = this.fixture.SendGetRequest(requestUrl);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [Then(@"Result contains single item with (.*) isoCode")]
        public void ThenListContainsItemWithIsoCode(string countryIsoCode)
        {
            var result = JsonConvert.DeserializeObject<List<CountryLocalisedResult>>(this.scenarioContext.GetResponseContent());

            result.Should().ContainSingle(x => x.Country.IsoCode == countryIsoCode);
        }
    }
}
