namespace KnightFrank.Antares.Api.IntegrationTests.Steps.CharacteristicGroups
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Dal.Model.Property.Characteristics;
    using KnightFrank.Antares.Dal.Model.Resource;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class CharacteristicGroupsSteps
    {
        private const string ApiUrl = "/api/characteristicGroups";
        private readonly BaseTestClassFixture fixture;

        private readonly ScenarioContext scenarioContext;

        public CharacteristicGroupsSteps(BaseTestClassFixture fixture, ScenarioContext scenarioContext)
        {
            this.fixture = fixture;
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            this.scenarioContext = scenarioContext;
        }

        [Given(@"User creates follwoing characteristics in database")]
        public void GivenIHaveEnteredFollowedCharacteristics(Table table)
        {
            IEnumerable<Characteristic> characteristics = table.CreateSet<Characteristic>();
            this.fixture.DataContext.Characteristics.AddRange(characteristics);
            this.fixture.DataContext.SaveChanges();
        }

        [Given(@"User retrieves (.*) country id")]
        public void GivenIHaveCountryId(string countryCode)
        {
            Country country = this.fixture.DataContext.Countries.SingleOrDefault(x => x.IsoCode == countryCode);
            this.scenarioContext["CountryId"] = country?.Id ?? Guid.NewGuid();
        }

        [When(@"User retrieves characteristics for given country and defined property type")]
        public void WhenUserRetrievesCharacteristicsGroupForCountryAndPropertyType()
        {
            var propertyTypeId = this.scenarioContext.Get<Guid>("PropertyTypeId");
            var countryId = this.scenarioContext.Get<Guid>("CountryId");

            this.ReciveCharacteristicsGroup(countryId, propertyTypeId);
        }

        [When(@"User tries to retrieves characteristics for (.*) country and (.*) property type")]
        public void RetrieveCharacteristicsGroupForCountryAndPropertyType(object country, string propertyTypeId)
        {
            propertyTypeId = propertyTypeId.Equals("valid")
                ? this.scenarioContext.Get<Guid>("PropertyTypeId").ToString()
                : propertyTypeId;

            object countryId = country.Equals("valid") ? this.scenarioContext.Get<Guid>("CountryId") : country;

            this.ReciveCharacteristicsGroup(countryId, propertyTypeId);
        }

        private void ReciveCharacteristicsGroup(object countryId, object propertyType)
        {
            string requestUrl = $"{ApiUrl}?countryId={countryId}&propertyTypeId={propertyType}";
            HttpResponseMessage response = this.fixture.SendGetRequest(requestUrl);

            this.scenarioContext.SetHttpResponseMessage(response);
        }
    }
}
