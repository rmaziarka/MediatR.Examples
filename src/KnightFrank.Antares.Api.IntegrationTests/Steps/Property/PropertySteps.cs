namespace KnightFrank.Antares.Api.IntegrationTests.Steps.Property
{
    using System;
    using System.Linq;
    using System.Net.Http;

    using FluentAssertions;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.Dal.Model.Property;

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

        [Given(@"Address is defined")]
        public void GivenAddressIsDefined(Table table)
        {
            var address = table.CreateInstance<Address>();
            address.AddressFormId = this.scenarioContext.Get<Guid>("AddressFormId");
            address.CountryId = this.scenarioContext.Get<Guid>("CountryId");
            this.scenarioContext.Set(address, "Address");
        }

        [Given(@"User gets (.*) address form for (.*) and country details")]
        [When(@"User gets (.*) address form for (.*) and country details")]
        public void GetCountryAddressData(string countryCode, string enumType)
        {
            Guid countryId = this.fixture.DataContext.Country.Single(country => country.IsoCode == countryCode).Id;
            Guid enumTypeId = this.fixture.DataContext.EnumTypeItem.Single(e => e.Code == enumType).Id;

            Guid addressFormId =
                this.fixture.DataContext.AddressFormEntityType.Single(
                    afe => afe.AddressForm.CountryId == countryId && afe.EnumTypeItemId == enumTypeId).AddressFormId;

            this.scenarioContext["CountryId"] = countryId;
            this.scenarioContext["AddressFormId"] = addressFormId;
        }

        [Given(@"Property with Address is in data base")]
        public void GivenFollowingPropertyExistsInDataBase(Table table)
        {
            var address = table.CreateInstance<Address>();
            address.AddressFormId = this.scenarioContext.Get<Guid>("AddressFormId");
            address.CountryId = this.scenarioContext.Get<Guid>("CountryId");
            var property = new Property { Address = address };

            this.fixture.DataContext.Property.Add(property);
            this.fixture.DataContext.SaveChanges();

            this.scenarioContext.Set(property.Id, "AddedPropertyId");
        }

        [When(@"Users updates property with defined address for (.*) id")]
        public void WhenUsersUpdatesProperty(string id)
        {
            Guid propertyId = id.Equals("latest") ? this.scenarioContext.Get<Guid>("AddedPropertyId") : new Guid(id);

            var address = this.scenarioContext.Get<Address>("Address");

            var updatedProperty = new Property
            {
                Address = address,
                Id = propertyId
            };
            this.scenarioContext.Set(updatedProperty, "Property");
            this.UpdateProperty(updatedProperty);
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

        [Then(@"The updated Property is saved in data base")]
        public void ThenTheResultsShouldBeSameAsAdded()
        {
            var updatedProperty = this.scenarioContext.Get<Property>("Property");
            Property expectedProperty = this.fixture.DataContext.Property.Single(x => x.Address.Id.Equals(updatedProperty.Id));
            updatedProperty.ShouldBeEquivalentTo(expectedProperty);
        }

        private void UpdateProperty(Property property)
        {
            HttpResponseMessage response = this.fixture.SendPutRequest(ApiUrl, property);
            this.scenarioContext.SetHttpResponseMessage(response);
        }
    }
}
