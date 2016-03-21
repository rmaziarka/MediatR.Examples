namespace KnightFrank.Antares.Api.IntegrationTests.Steps.AddressForm
{
    using System;
    using System.Linq;
    using System.Net.Http;

    using FluentAssertions;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Dal.Model;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    using Xunit;

    [Binding]
    public class AddressFormSteps : IClassFixture<BaseTestClassFixture>
    {
        private const string ApiUrl = "/api/AddressForm";
        private readonly BaseTestClassFixture fixture;

        private readonly ScenarioContext scenarioContext;
        private AddressField AddressField { get; set; }
        private AddressFieldLabel AddressFieldLabel { get; set; }
        private AddressForm AddressForm { get; set; }
        private AddressFieldDefinition AddressFieldDefinition { get; set; }

        public AddressFormSteps(BaseTestClassFixture fixture, ScenarioContext scenarioContext)
        {
            this.fixture = fixture;
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            this.scenarioContext = scenarioContext;
        }

        [Given(@"Country code (.*) is present in DB")]
        public void GivenCountryCodeIsPresentInDb(string countryCode)
        {
            var country = new Country { IsoCode = countryCode };
            this.fixture.DataContext.Country.Add(country);
            this.fixture.DataContext.SaveChanges();
        }

        [When(@"User retrieves address template for (.*) entity type and (.*) contry code")]
        public void WhenUserTryToRetrieveContactsDetailsForFollowingData(string enumTypeItem, string countryCode)
        {
            string requestUrl = $"{ApiUrl}?entityType=" + enumTypeItem + "&countryCode=" + countryCode + "";
            HttpResponseMessage response = this.fixture.SendGetRequest(requestUrl);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [Given(@"There is AddressForm for (.*) country code")]
        public void GivenThereIsAddressFormForUk(string countryIsoCode)
        {
            Country country = this.fixture.DataContext.Country.FirstOrDefault(c => c.IsoCode == countryIsoCode);
            this.AddressForm = new AddressForm { Country = country };

            this.fixture.DataContext.AddressForm.Add(this.AddressForm);
            this.fixture.DataContext.SaveChanges();
        }
    }
}
