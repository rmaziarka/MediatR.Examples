namespace KnightFrank.Antares.Api.IntegrationTests.Steps.Property
{
    using System;
    using System.Linq;
    using System.Net.Http;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Dal.Model;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

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

        [When(@"User gets (.*) address form for (.*) and country details")]
        public void GetCountryAddressData(string countryCode, string enumType)
        {
            Guid countryId = this.fixture.DataContext.Country.Single(country => country.IsoCode == countryCode).Id;
            Guid enumTypeId = this.fixture.DataContext.EnumTypeItem.Single(e => e.Code == enumType).Id;

            Guid addressFormId =
                this.fixture.DataContext.AddressFormEntityType.Single(
                    afe => afe.AddressForm.CountryId == countryId && afe.EnumTypeItemId == enumTypeId).Id;

            this.scenarioContext["CountryId"] = countryId;
            this.scenarioContext["AddressFormId"] = addressFormId;
        }

        [When(@"User creates property with following data")]
        public void CreateProperty(Table table)
        {
            string requestUrl = $"{ApiUrl}";

            var propertyDetails = table.CreateInstance<Address>();
            propertyDetails.AddressFormId = this.scenarioContext.Get<Guid>("AddressFormId");
            propertyDetails.CountryId = this.scenarioContext.Get<Guid>("CountryId");

            var property = new Property { Address = propertyDetails };

            HttpResponseMessage response = this.fixture.SendPostRequest(requestUrl, property);
            this.scenarioContext.SetHttpResponseMessage(response);
        }
    }
}
