namespace KnightFrank.Antares.Api.IntegrationTests.Steps.Property
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    using FluentAssertions;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.Dal.Model.Attribute;
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
        private const string ApiUrl = "/api/properties";
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

        [Given(@"Property does not exist in DB")]
        public void GivenPropertyDoesNotExistsInDataBase()
        {
            this.scenarioContext.Set(Guid.NewGuid(), "AddedPropertyId");
        }

        [Given(@"Address for add/update property is defined")]
        public void AddressIsDefined(Table table)
        {
            var address = table.CreateInstance<CreateOrUpdatePropertyAddress>();
            address.AddressFormId = this.scenarioContext.Get<Guid>("AddressFormId");
            address.CountryId = this.scenarioContext.Get<Guid>("CountryId");

            this.scenarioContext.Set(address, "Address");
        }

        [Given(@"Address for add/update property is defined with max length fields")]
        public void AddressIsDefinedWithMaxFields()
        {
            var address = new CreateOrUpdatePropertyAddress
            {
                PropertyName = StringExtension.GenerateMaxAlphanumericString(128),
                PropertyNumber = StringExtension.GenerateMaxAlphanumericString(8),
                Line2 = StringExtension.GenerateMaxAlphanumericString(128),
                Line3 = StringExtension.GenerateMaxAlphanumericString(128),
                Postcode = StringExtension.GenerateMaxAlphanumericString(10),
                City = StringExtension.GenerateMaxAlphanumericString(128),
                County = StringExtension.GenerateMaxAlphanumericString(128),
                AddressFormId = this.scenarioContext.Get<Guid>("AddressFormId"),
                CountryId = this.scenarioContext.Get<Guid>("CountryId")
            };

            this.scenarioContext.Set(address, "Address");
        }

        [Given(@"User gets (.*) address form for (.*) and country details")]
        [When(@"User gets (.*) address form for (.*) and country details")]
        public void GetCountryAddressData(string countryCode, string enumType)
        {
            Country country = this.fixture.DataContext.Countries.SingleOrDefault(x => x.IsoCode == countryCode);
            EnumTypeItem enumTypeItem = this.fixture.DataContext.EnumTypeItems.SingleOrDefault(e => e.Code == enumType);
            Guid countryId = country?.Id ?? Guid.NewGuid();
            Guid enumTypeId = enumTypeItem?.Id ?? Guid.NewGuid();

            AddressFormEntityType addressForm = this.fixture.DataContext.AddressFormEntityTypes.SingleOrDefault(
                afe => afe.AddressForm.CountryId == countryId && afe.EnumTypeItemId == enumTypeId);

            Guid addressFormId = addressForm?.AddressFormId ?? Guid.NewGuid();

            this.scenarioContext["CountryId"] = countryId;
            this.scenarioContext["AddressFormId"] = addressFormId;
        }

        [Given(@"Property with Address and (.*) division is in database")]
        public void GivenFollowingPropertyExistsInDataBase(string divisionCode, Table table)
        {
            var address = table.CreateInstance<Address>();
            var propertyTypeId = this.scenarioContext.Get<Guid>("PropertyTypeId");
            var attributeValues = this.scenarioContext.Get<AttributeValues>("AttributeValues");

            Guid divisionId = this.scenarioContext.Get<Dictionary<string, Guid>>("EnumDictionary")[divisionCode];

            address.AddressFormId = this.scenarioContext.Get<Guid>("AddressFormId");
            address.CountryId = this.scenarioContext.Get<Guid>("CountryId");

            var property = new Property
            {
                Address = address,
                PropertyTypeId = propertyTypeId,
                DivisionId = divisionId,
                AttributeValues = attributeValues
            };

            this.fixture.DataContext.Properties.Add(property);
            this.fixture.DataContext.SaveChanges();

            this.scenarioContext.Set(property.Id, "AddedPropertyId");
        }

        [Given(@"User gets (.*) for PropertyType")]
        public void GetPropertyTypeId(string propertyTypeCode)
        {
            if (propertyTypeCode.Equals("invalid"))
            {
                this.scenarioContext.Set(Guid.NewGuid(), "PropertyTypeId");
            }
            else
            {
                Guid propertyTypeId = this.fixture.DataContext.PropertyTypes.Single(i => i.Code.Equals(propertyTypeCode)).Id;
                this.scenarioContext.Set(propertyTypeId, "PropertyTypeId");
            }
        }

        [Given(@"User sets attributes for property in database")]
        public void SetAttributesForPropertyForDb(Table table)
        {
            var attributeValues = table.CreateInstance<AttributeValues>();
            this.scenarioContext.Set(attributeValues, "AttributeValues");
        }

        [Given(@"User sets attributes for property in Api")]
        public void SetAttributesForPropertyForApi(Table table)
        {
            var attributeValues = table.CreateInstance<CreateOrUpdatePropertyAttributeValues>();
            this.scenarioContext.Set(attributeValues, "AttributeValues");
        }

        [Given(@"Property has following charactersitics")]
        public void GivenPropertyHasFollowingCharactersitics(Table table)
        {
            var id = this.scenarioContext.Get<Guid>("AddedPropertyId");
            IEnumerable<RequiredCharacteristics> propertyCharacteristic = table.CreateSet<RequiredCharacteristics>();

            foreach (PropertyCharacteristic prop in propertyCharacteristic.Select(characteristic => new PropertyCharacteristic
            {
                CharacteristicId = this.fixture.DataContext.Characteristics.Single(x => x.Code == characteristic.CharacteristicCode).Id,
                PropertyId = id,
                Text = characteristic.Text,
            }))
            {
                this.fixture.DataContext.Properties.Single(x => x.Id == id).PropertyCharacteristics.Add(prop);
            }
            this.fixture.DataContext.SaveChanges();
        }

        [When(@"Users updates property with defined address for (.*) id and (.*) division by Api")]
        public void WhenUsersUpdatesProperty(string id, string divisionCode)
        {
            string requestUrl = $"{ApiUrl}";

            var address = this.scenarioContext.Get<CreateOrUpdatePropertyAddress>("Address");
            var propertyTypeId = this.scenarioContext.Get<Guid>("PropertyTypeId");
            var attributeValues = this.scenarioContext.Get<CreateOrUpdatePropertyAttributeValues>("AttributeValues");

            Guid propertyId = id.Equals("latest") ? this.scenarioContext.Get<Guid>("AddedPropertyId") : new Guid(id);
            Guid divisionId = this.scenarioContext.Get<Dictionary<string, Guid>>("EnumDictionary")[divisionCode];

            var updatedProperty = new UpdatePropertyCommand
            {
                Address = address,
                Id = propertyId,
                PropertyTypeId = propertyTypeId,
                DivisionId = divisionId,
                AttributeValues = attributeValues
            };

            HttpResponseMessage response = this.fixture.SendPutRequest(requestUrl, updatedProperty);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [When(@"User creates property with defined address and (.*) division by Api")]
        public void CreateProperty(string divisionCode)
        {
            string requestUrl = $"{ApiUrl}";

            var address = this.scenarioContext.Get<CreateOrUpdatePropertyAddress>("Address");
            var propertyTypeId = this.scenarioContext.Get<Guid>("PropertyTypeId");
            var attributeValues = this.scenarioContext.Get<CreateOrUpdatePropertyAttributeValues>("AttributeValues");

            Guid divisionId = this.scenarioContext.Get<Dictionary<string, Guid>>("EnumDictionary")[divisionCode];

            var property = new CreatePropertyCommand
            {
                Address = address,
                PropertyTypeId = propertyTypeId,
                DivisionId = divisionId,
                AttributeValues = attributeValues
            };

            HttpResponseMessage response = this.fixture.SendPostRequest(requestUrl, property);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [When(@"User retrieves property details")]
        public void GetProperty()
        {
            var propertyId = this.scenarioContext.Get<Guid>("AddedPropertyId");
            string requestUrl = $"{ApiUrl}/{propertyId}";

            HttpResponseMessage response = this.fixture.SendGetRequest(requestUrl);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [When(@"User retrieves attributes for given property type and (.*) address")]
        public void GetAttributesForPropertyAndCountry(string countryCode)
        {
            var propertyTypeId = this.scenarioContext.Get<Guid>("PropertyTypeId");
            string requestUrl = $"{ApiUrl}/attributes?countryCode={countryCode}&propertyTypeId={propertyTypeId}";

            HttpResponseMessage response = this.fixture.SendGetRequest(requestUrl);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [Then(@"The updated Property is saved in database")]
        public void ThenTheResultsShouldBeSameAsAdded()
        {
            var updatedProperty = JsonConvert.DeserializeObject<Property>(this.scenarioContext.GetResponseContent());
            Property actualProperty = this.fixture.DataContext.Properties.SingleOrDefault(x => x.Id.Equals(updatedProperty.Id));

            updatedProperty.ShouldBeEquivalentTo(actualProperty, options => options
                .Excluding(x => x.Division)
                .Excluding(x => x.PropertyType)
                .Excluding(x => x.Address.AddressForm)
                .Excluding(x => x.Address.Country));
        }

        [Then(@"The created Property is saved in database")]
        public void ThenTheResultsShouldBeSameAsCreated()
        {
            var expectedProperty = JsonConvert.DeserializeObject<Property>(this.scenarioContext.GetResponseContent());
            Property actualProperty = this.fixture.DataContext.Properties.Single(x => x.Id.Equals(expectedProperty.Id));

            actualProperty.ShouldBeEquivalentTo(expectedProperty, options => options
                .Excluding(x => x.Activities)
                .Excluding(x => x.Ownerships)
                .Excluding(x => x.Division)
                .Excluding(x => x.Address.AddressForm)
                .Excluding(x => x.Address.Country)
                .Excluding(x => x.PropertyType)
                .Excluding(x => x.PropertyCharacteristics));
        }

        [Then(@"Characteristics list should be the same as in database")]
        public void ThenCharacteristicsListShouldBeTheSameAsInDatabase()
        {
            var propertyTypeId = this.scenarioContext.Get<Guid>("AddedPropertyId");
            List<PropertyCharacteristic> expectedCharacteristics = JsonConvert.DeserializeObject<Property>(this.scenarioContext.GetResponseContent()).PropertyCharacteristics.ToList();
            List<PropertyCharacteristic> actualCharacteristics = this.fixture.DataContext.Properties.Single(x => x.Id.Equals(propertyTypeId)).PropertyCharacteristics.ToList();

            for (var i = 0; i < actualCharacteristics.Count; i++)
            {
                actualCharacteristics[i].CharacteristicId.ShouldBeEquivalentTo(expectedCharacteristics[i].CharacteristicId);
                actualCharacteristics[i].PropertyId.ShouldBeEquivalentTo(expectedCharacteristics[i].PropertyId);
                actualCharacteristics[i].Text.ShouldBeEquivalentTo(expectedCharacteristics[i].Text);
                actualCharacteristics[i].Id.ShouldBeEquivalentTo(expectedCharacteristics[i].Id);
            }
        }


        internal class RequiredCharacteristics
        {
            public string CharacteristicCode { get; set; }
            public string Text { get; set; }
        }
    }
}
