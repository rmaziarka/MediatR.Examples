namespace KnightFrank.Antares.Api.IntegrationTests.Steps.CharacteristicGroups
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Dal.Model.Property;
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

        [Given(@"I have entered followed characteristics")]
        public void GivenIHaveEnteredFollowedCharacteristics(Table table)
        {
            IEnumerable<Characteristic> characteristics = table.CreateSet<Characteristic>();
            this.fixture.DataContext.Characteristics.AddRange(characteristics);
            this.fixture.DataContext.SaveChanges();
        }

        [Given(@"I have (.*) country id")]
        public void GivenIHaveCountryId(string countryCode)
        {
            Country country = this.fixture.DataContext.Countries.SingleOrDefault(x => x.IsoCode == countryCode);
            this.scenarioContext["CountryId"] = country?.Id ?? Guid.NewGuid();
        }

        [Given(@"I have (.*) property type id")]
        public void GivenIHavePropertyTypeId(string type)
        {
            PropertyType propertyType = this.fixture.DataContext.PropertyTypes.SingleOrDefault(x => x.Code == type);
            this.scenarioContext["PropertyTypeId"] = propertyType?.Id ?? Guid.NewGuid();
        }

        [Given(@"And I have entered followed characteristics groups")]
        public void GivenAndIHaveEnteredFollowedCharacteristicsGroups(Table table)
        {
            IEnumerable<CharacteristicGroup> characteristicGroup = table.CreateSet<CharacteristicGroup>();
            this.fixture.DataContext.CharacteristicGroups.AddRange(characteristicGroup);
            this.fixture.DataContext.SaveChanges();
            this.scenarioContext.Set(characteristicGroup, "CharacteristicGroup");
        }

        [When(@"User retrieves characteristics for (.*) country and defined property type")]
        public void WhenUserRetrievesCharacteristicsGroupForCountryAndPropertyType(string country)
        {
            var propertyTypeId = this.scenarioContext.Get<Guid>("PropertyTypeId");

            this.ReciveCharacteristicsGroup(country, propertyTypeId);
        }

        [When(@"User try to retrieves characteristics for (.*) country and (.*) property type")]
        public void WhenUserTryToRetrieveCharacteristicsGroupForCountryAndPropertyType(string countryCodeId, string propertyTypeId)
        {
            propertyTypeId = propertyTypeId.Equals("proper")
                ? this.scenarioContext.Get<Guid>("PropertyTypeId").ToString()
                : propertyTypeId;

            this.ReciveCharacteristicsGroup(countryCodeId, propertyTypeId);
        }

        private void ReciveCharacteristicsGroup(object countryCode, object propertyType)
        {
            string requestUrl = $"{ApiUrl}?countryCode=" + countryCode + "&propertyTypeId=" + propertyType + "";
            HttpResponseMessage response = this.fixture.SendGetRequest(requestUrl);

            this.scenarioContext.SetHttpResponseMessage(response);
        }
    }
}
