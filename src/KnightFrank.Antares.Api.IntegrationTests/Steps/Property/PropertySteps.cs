namespace KnightFrank.Antares.Api.IntegrationTests.Steps.Property
{
    using System;
    using System.Linq;
    using System.Net.Http;

    using FluentAssertions;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Resource;
    using KnightFrank.Antares.Domain.Property.Commands;

    using Newtonsoft.Json;

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

        [Given(@"Address for add/update property is defined")]
        public void GivenAddressIsDefined(Table table)
        {
            var address = table.CreateInstance<CreateOrUpdatePropertyAddress>();
            address.AddressFormId = this.scenarioContext.Get<Guid>("AddressFormId");
            address.CountryId = this.scenarioContext.Get<Guid>("CountryId");
            this.scenarioContext.Set(address, "Address");
        }

        [Given(@"User gets (.*) address form for (.*) and country details")]
        [When(@"User gets (.*) address form for (.*) and country details")]
        public void GetCountryAddressData(string countryCode, string enumType)
        {
            Country country = this.fixture.DataContext.Country.SingleOrDefault(x => x.IsoCode == countryCode);
            EnumTypeItem enumTypeItem = this.fixture.DataContext.EnumTypeItem.SingleOrDefault(e => e.Code == enumType);
            Guid countryId = country?.Id ?? new Guid();
            Guid enumTypeId = enumTypeItem?.Id ?? new Guid();

            AddressFormEntityType addressForm = this.fixture.DataContext.AddressFormEntityType.SingleOrDefault(
                afe => afe.AddressForm.CountryId == countryId && afe.EnumTypeItemId == enumTypeId);

            Guid addressFormId = addressForm?.AddressFormId ?? new Guid();

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

        [When(@"Users updates property with defined address for (.*) id by Api")]
        public void WhenUsersUpdatesProperty(string id)
        {
            Guid propertyId = id.Equals("latest") ? this.scenarioContext.Get<Guid>("AddedPropertyId") : new Guid(id);

            var address = this.scenarioContext.Get<CreateOrUpdatePropertyAddress>("Address");

            var updatedProperty = new UpdatePropertyCommand { Address = address, Id = propertyId };

            HttpResponseMessage response = this.fixture.SendPutRequest(ApiUrl, updatedProperty);
            this.scenarioContext.SetHttpResponseMessage(response);
            this.scenarioContext.Set(updatedProperty, "Property");
        }

        [When(@"User creates property with defined address by Api")]
        public void CreateProperty()
        {
            string requestUrl = $"{ApiUrl}";

            var address = this.scenarioContext.Get<CreateOrUpdatePropertyAddress>("Address");

            address.AddressFormId = this.scenarioContext.Get<Guid>("AddressFormId");
            address.CountryId = this.scenarioContext.Get<Guid>("CountryId");

            var property = new CreatePropertyCommand { Address = address };

            HttpResponseMessage response = this.fixture.SendPostRequest(requestUrl, property);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [Then(@"The updated Property is saved in data base")]
        public void ThenTheResultsShouldBeSameAsAdded()
        {
            var updatedProperty = this.scenarioContext.Get<UpdatePropertyCommand>("Property");
            Property expectedProperty = this.fixture.DataContext.Property.SingleOrDefault(x => x.Id.Equals(updatedProperty.Id));
            updatedProperty.ShouldBeEquivalentTo(expectedProperty);
        }

        [Then(@"The created Property is saved in data base")]
        public void ThenTheResultsShouldBeSameAsCreated()
        {
            var address = this.scenarioContext.Get<CreateOrUpdatePropertyAddress>("Address");
            var expectedProperty = JsonConvert.DeserializeObject<Property>(this.scenarioContext.GetResponseContent());
            Property actualProperty = this.fixture.DataContext.Property.Single(x => x.Id.Equals(expectedProperty.Id));

            actualProperty.ShouldBeEquivalentTo(expectedProperty, options => options
                .Excluding(x => x.Ownerships)
                .Excluding(x => x.Address.AddressForm)
                .Excluding(x => x.Address.Country)
                .Excluding(x => x.Address.Line1));
        }
    }
}
