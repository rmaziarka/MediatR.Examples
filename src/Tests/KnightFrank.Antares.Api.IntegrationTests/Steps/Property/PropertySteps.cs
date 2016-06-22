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
    using KnightFrank.Antares.Dal.Model.Attachment;
    using KnightFrank.Antares.Dal.Model.Attribute;
    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Characteristics;
    using KnightFrank.Antares.Dal.Model.Resource;
    using KnightFrank.Antares.Domain.Common.Commands;
    using KnightFrank.Antares.Domain.Common.Enums;
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

        [Given(@"Address for add/update property is defined")]
        public void AddressIsDefined(Table table)
        {
            var address = table.CreateInstance<CreateOrUpdateAddress>();
            address.AddressFormId = this.scenarioContext.Get<Guid>("AddressFormId");
            address.CountryId = this.scenarioContext.Get<Guid>("CountryId");

            this.scenarioContext.Set(address, "Address");
        }

        [Given(@"Address for add/update property is defined with max length fields")]
        public void AddressIsDefinedWithMaxFields()
        {
            var address = new CreateOrUpdateAddress
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
        public void GetCountryAddressData(string countryCode, string enumType)
        {
            Country country = this.fixture.DataContext.Countries.SingleOrDefault(x => x.IsoCode.Equals(countryCode));
            EnumTypeItem enumTypeItem =
                this.fixture.DataContext.EnumTypeItems.SingleOrDefault(
                    e => e.EnumType.Code.Equals(nameof(EntityType)) && e.Code.Equals(enumType));
            Guid countryId = country?.Id ?? Guid.NewGuid();
            Guid enumTypeId = enumTypeItem?.Id ?? Guid.NewGuid();

            AddressFormEntityType addressForm = this.fixture.DataContext.AddressFormEntityTypes.SingleOrDefault(
                afe => afe.AddressForm.CountryId.Equals(countryId) && afe.EnumTypeItemId.Equals(enumTypeId));

            Guid addressFormId = addressForm?.AddressFormId ?? Guid.NewGuid();

            this.scenarioContext["CountryId"] = countryId;
            this.scenarioContext["AddressFormId"] = addressFormId;
        }

        [Given(@"Property exists in database")]
        public void CreatePropertyInDatabase(Table table)
        {
            string propertyType = table.Rows[0]["PropertyType"];
            string division = table.Rows[0]["Division"];

            const string countryCode = "GB";
            Guid countryId = this.fixture.DataContext.Countries.Single(x => x.IsoCode.Equals(countryCode)).Id;
            Guid enumTypeItemId =
                this.fixture.DataContext.EnumTypeItems.Single(
                    e => e.EnumType.Code.Equals(nameof(EntityType)) && e.Code.Equals(nameof(EntityType.Property))).Id;
            Guid addressFormId =
                this.fixture.DataContext.AddressFormEntityTypes.Single(
                    afe => afe.AddressForm.CountryId.Equals(countryId) && afe.EnumTypeItemId.Equals(enumTypeItemId)).AddressFormId;

            Guid propertyTypeId = this.fixture.DataContext.PropertyTypes.Single(i => i.Code.Equals(propertyType)).Id;
            Guid divisionId =
                this.fixture.DataContext.EnumTypeItems.Single(
                    i => i.EnumType.Code.Equals(nameof(Division)) && i.Code.Equals(division)).Id;

            AttributeValues attributeValues = this.scenarioContext.Keys.Contains("AttributeValues")
                ? this.scenarioContext.Get<AttributeValues>("AttributeValues")
                : new AttributeValues();

            List<CreateOrUpdatePropertyCharacteristic> createOrUpdatePropertyCharacteristic =
                this.scenarioContext.Keys.Contains("PropertyCharacteristics")
                    ? this.scenarioContext.Get<List<CreateOrUpdatePropertyCharacteristic>>("PropertyCharacteristics")
                    : new List<CreateOrUpdatePropertyCharacteristic>();

            List<PropertyCharacteristic> propertyCharacteristic =
                createOrUpdatePropertyCharacteristic.Select(
                    prop => new PropertyCharacteristic { CharacteristicId = prop.CharacteristicId, Text = prop.Text }).ToList();

            var address = new Address
            {
                PropertyName = StringExtension.GenerateMaxAlphanumericString(128),
                PropertyNumber = StringExtension.GenerateMaxAlphanumericString(8),
                Line1 = StringExtension.GenerateMaxAlphanumericString(128),
                Line2 = StringExtension.GenerateMaxAlphanumericString(128),
                Line3 = StringExtension.GenerateMaxAlphanumericString(128),
                Postcode = StringExtension.GenerateMaxAlphanumericString(10),
                City = StringExtension.GenerateMaxAlphanumericString(128),
                County = StringExtension.GenerateMaxAlphanumericString(128),
                AddressFormId = addressFormId,
                CountryId = countryId
            };

            var property = new Property
            {
                Address = address,
                PropertyTypeId = propertyTypeId,
                DivisionId = divisionId,
                AttributeValues = attributeValues,
                PropertyCharacteristics = propertyCharacteristic,
                Attachments = new List<Attachment>()
            };

            this.fixture.DataContext.Properties.Add(property);
            this.fixture.DataContext.SaveChanges();

            this.scenarioContext.Set(property, "Property");
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

        [Given(@"Property attributes exists in database")]
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
            Guid id = this.scenarioContext.Get<Property>("Property").Id;
            IEnumerable<RequiredCharacteristics> propertyCharacteristic = table.CreateSet<RequiredCharacteristics>();

            foreach (PropertyCharacteristic prop in propertyCharacteristic.Select(characteristic => new PropertyCharacteristic
            {
                CharacteristicId =
                    this.fixture.DataContext.Characteristics.Single(x => x.Code == characteristic.CharacteristicCode).Id,
                PropertyId = id,
                Text = characteristic.Text
            }))
            {
                this.fixture.DataContext.Properties.Single(x => x.Id == id).PropertyCharacteristics.Add(prop);
            }
            this.fixture.DataContext.SaveChanges();
        }

        [Given(@"Property characteristics are set for given property type")]
        public void GivenPropertyCharacteristicsAreSetForGivenPropertyType()
        {
            var propertyTypeId = this.scenarioContext.Get<Guid>("PropertyTypeId");
            var countryId = this.scenarioContext.Get<Guid>("CountryId");
            var list = new List<Guid>();

            foreach (
                Guid id in
                    this.fixture.DataContext.CharacteristicGroupUsages.Where(
                        x => x.PropertyTypeId.Equals(propertyTypeId) && x.CountryId.Equals(countryId)).Select(x => x.Id).ToList())
            {
                list.AddRange(this.fixture.DataContext.CharacteristicGroupItems.Where(x => x.CharacteristicGroupUsageId.Equals(id))
                                  .Select(x => x.CharacteristicId));
            }

            this.scenarioContext.Set(
                list.Select(
                    id =>
                        new CreateOrUpdatePropertyCharacteristic
                        {
                            CharacteristicId = id,
                            Text = StringExtension.GenerateMaxAlphanumericString(50)
                        }).ToList(),
                "PropertyCharacteristics");
        }

        [Given(@"Followed property characteristics are chosen")]
        public void GivenFollowedPropertyCharacteristicsAreChosen(Table table)
        {
            var characteristicList = new List<CreateOrUpdatePropertyCharacteristic>();
            var propertyCharacteristic = table.CreateInstance<Characteristic>();

            Characteristic characteristic =
                this.fixture.DataContext.Characteristics.SingleOrDefault(x => x.Code == propertyCharacteristic.Code);

            characteristicList.Add(new CreateOrUpdatePropertyCharacteristic
            {
                CharacteristicId = characteristic?.Id ?? new Guid(),
                Text = StringExtension.GenerateMaxAlphanumericString(50)
            });

            this.scenarioContext.Set(characteristicList, "PropertyCharacteristics");
        }

        [When(@"User updates property with defined address for (.*) id and (.*) division by Api")]
        public void WhenUsersUpdatesProperty(string id, string divisionCode)
        {
            string requestUrl = $"{ApiUrl}";

            var address = this.scenarioContext.Get<CreateOrUpdateAddress>("Address");
            var propertyTypeId = this.scenarioContext.Get<Guid>("PropertyTypeId");

            CreateOrUpdatePropertyAttributeValues attributeValues = this.scenarioContext.Keys.Contains("AttributeValues")
                ? this.scenarioContext.Get<CreateOrUpdatePropertyAttributeValues>("AttributeValues")
                : new CreateOrUpdatePropertyAttributeValues();

            List<CreateOrUpdatePropertyCharacteristic> propertyCharacteristics =
                this.scenarioContext.Keys.Contains("PropertyCharacteristics")
                    ? this.scenarioContext.Get<List<CreateOrUpdatePropertyCharacteristic>>("PropertyCharacteristics")
                    : new List<CreateOrUpdatePropertyCharacteristic>();

            Guid propertyId = id.Equals("latest") ? this.scenarioContext.Get<Property>("Property").Id : new Guid(id);
            Guid divisionId = this.scenarioContext.Get<Dictionary<string, Guid>>("EnumDictionary")[divisionCode];

            var updatedProperty = new UpdatePropertyCommand
            {
                Address = address,
                Id = propertyId,
                PropertyTypeId = propertyTypeId,
                DivisionId = divisionId,
                AttributeValues = attributeValues,
                PropertyCharacteristics = propertyCharacteristics
            };

            HttpResponseMessage response = this.fixture.SendPutRequest(requestUrl, updatedProperty);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [When(@"User creates property with defined address and (.*) division by Api")]
        public void CreateProperty(string divisionCode)
        {
            var address = this.scenarioContext.Get<CreateOrUpdateAddress>("Address");
            var propertyTypeId = this.scenarioContext.Get<Guid>("PropertyTypeId");

            CreateOrUpdatePropertyAttributeValues attributeValues = this.scenarioContext.Keys.Contains("AttributeValues")
                ? this.scenarioContext.Get<CreateOrUpdatePropertyAttributeValues>("AttributeValues")
                : new CreateOrUpdatePropertyAttributeValues();

            List<CreateOrUpdatePropertyCharacteristic> propertyCharacteristics =
                this.scenarioContext.Keys.Contains("PropertyCharacteristics")
                    ? this.scenarioContext.Get<List<CreateOrUpdatePropertyCharacteristic>>("PropertyCharacteristics")
                    : new List<CreateOrUpdatePropertyCharacteristic>();

            Guid divisionId = this.scenarioContext.Get<Dictionary<string, Guid>>("EnumDictionary")[divisionCode];

            var property = new CreatePropertyCommand
            {
                Address = address,
                PropertyTypeId = propertyTypeId,
                DivisionId = divisionId,
                AttributeValues = attributeValues,
                PropertyCharacteristics = propertyCharacteristics
            };

            this.CreateProperty(property);
        }

        [When(@"User creates property with defined address and (.*) division with mandatory fields by Api")]
        public void CreatePropertyWithMandatoryFields(string divisionCode)
        {
            var propertyTypeId = this.scenarioContext.Get<Guid>("PropertyTypeId");
            Guid divisionId = this.scenarioContext.Get<Dictionary<string, Guid>>("EnumDictionary")[divisionCode];

            var property = new CreatePropertyCommand
            {
                PropertyTypeId = propertyTypeId,
                DivisionId = divisionId,
                AttributeValues = new CreateOrUpdatePropertyAttributeValues(),
                Address = new CreateOrUpdateAddress
                {
                    AddressFormId = this.scenarioContext.Get<Guid>("AddressFormId"),
                    CountryId = this.scenarioContext.Get<Guid>("CountryId"),
                    Postcode = StringExtension.GenerateMaxAlphanumericString(10),
                    City = string.Empty,
                    County = string.Empty,
                    Line2 = string.Empty,
                    Line3 = string.Empty,
                    PropertyName = string.Empty,
                    PropertyNumber = string.Empty
                }
            };

            this.CreateProperty(property);
        }

        [When(@"User retrieves property details")]
        public void GetProperty()
        {
            Guid propertyId = this.scenarioContext.ContainsKey("Property")
                ? this.scenarioContext.Get<Property>("Property").Id
                : Guid.NewGuid();
            string requestUrl = $"{ApiUrl}/{propertyId}";

            HttpResponseMessage response = this.fixture.SendGetRequest(requestUrl);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [When(@"User retrieves attributes for given property type id and country id")]
        public void GetAttributesForPropertyAndCountry()
        {
            var propertyTypeId = this.scenarioContext.Get<Guid>("PropertyTypeId");
            var countryId = this.scenarioContext.Get<Guid>("CountryId");
            string requestUrl = $"{ApiUrl}/attributes?countryId={countryId}&propertyTypeId={propertyTypeId}";

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
                .Excluding(x => x.Address.Country)
                .Excluding(x => x.PropertyCharacteristics)
                .Excluding(x => x.Attachments));

            updatedProperty.PropertyCharacteristics.Should()
                           .Equal(actualProperty?.PropertyCharacteristics,
                               (c1, c2) =>
                                   c1.CharacteristicId == c2.CharacteristicId &&
                                   c1.PropertyId == c2.PropertyId &&
                                   c1.Text == c2.Text &&
                                   c1.Id == c2.Id);
        }

        [Then(@"The created Property is saved in database")]
        public void ThenTheResultsShouldBeSameAsCreated()
        {
            var expectedProperty = JsonConvert.DeserializeObject<Property>(this.scenarioContext.GetResponseContent());
            Property actualProperty = this.fixture.DataContext.Properties.SingleOrDefault(x => x.Id.Equals(expectedProperty.Id));

            actualProperty.ShouldBeEquivalentTo(expectedProperty, options => options
                .Excluding(x => x.Activities)
                .Excluding(x => x.Ownerships)
                .Excluding(x => x.Division)
                .Excluding(x => x.Address.AddressForm)
                .Excluding(x => x.Address.Country)
                .Excluding(x => x.PropertyType)
                .Excluding(x => x.PropertyCharacteristics)
                .Excluding(x => x.Attachments));

            expectedProperty.PropertyCharacteristics.Should()
                            .Equal(actualProperty?.PropertyCharacteristics,
                                (c1, c2) =>
                                    c1.CharacteristicId == c2.CharacteristicId &&
                                    c1.PropertyId == c2.PropertyId &&
                                    c1.Text == c2.Text &&
                                    c1.Id == c2.Id);
        }

        [Then(@"Characteristics list should be the same as in database")]
        public void ThenCharacteristicsListShouldBeTheSameAsInDatabase()
        {
            Guid propertyTypeId = this.scenarioContext.Get<Property>("Property").Id;
            List<PropertyCharacteristic> expectedCharacteristics =
                JsonConvert.DeserializeObject<Property>(this.scenarioContext.GetResponseContent()).PropertyCharacteristics.ToList();
            List<PropertyCharacteristic> actualCharacteristics =
                this.fixture.DataContext.Properties.Single(x => x.Id.Equals(propertyTypeId)).PropertyCharacteristics.ToList();

            actualCharacteristics.Should()
                                 .Equal(expectedCharacteristics,
                                     (c1, c2) =>
                                         c1.CharacteristicId == c2.CharacteristicId &&
                                         c1.PropertyId == c2.PropertyId &&
                                         c1.Text == c2.Text &&
                                         c1.Id == c2.Id);
        }

        private void CreateProperty(CreatePropertyCommand command)
        {
            string requestUrl = $"{ApiUrl}";
            HttpResponseMessage response = this.fixture.SendPostRequest(requestUrl, command);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        internal class RequiredCharacteristics
        {
            public string CharacteristicCode { get; set; }
            public string Text { get; set; }
        }
    }
}
