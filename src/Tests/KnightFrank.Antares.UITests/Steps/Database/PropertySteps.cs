namespace KnightFrank.Antares.UITests.Steps.Database
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal;
    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.Dal.Model.Attribute;
    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.UITests.Extensions;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class PropertySteps
    {
        private readonly ScenarioContext scenarioContext;

        public PropertySteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }

            this.scenarioContext = scenarioContext;
        }

        [Given(@"Property in GB is created in database")]
        public void CreatePropertyInDb(Table table)
        {
            var address = table.CreateInstance<Address>();
            KnightFrankContext dataContext = DatabaseExtensions.GetDataContext();

            // Get country and address form id
            Guid countryId = dataContext.Countries.Single(x => x.IsoCode == "GB").Id;
            Guid enumTypeId = dataContext.EnumTypeItems.Single(e => e.Code == "Property").Id;
            Guid addressFormId =
                dataContext.AddressFormEntityTypes.Single(
                    afe => afe.AddressForm.CountryId == countryId && afe.EnumTypeItemId == enumTypeId).AddressFormId;

            // Get property type id and division id
            var propertyTypeId = this.scenarioContext.Get<Guid>("PropertyTypeId");
            var divisionId = this.scenarioContext.Get<Guid>("DivisionId");

            // Set property details
            address.AddressFormId = addressFormId;
            address.CountryId = countryId;

            var property = new Property
            {
                Address = address,
                PropertyTypeId = propertyTypeId,
                DivisionId = divisionId,
                AttributeValues =
                    this.scenarioContext.Keys.Contains("AttributeValues")
                        ? this.scenarioContext.Get<AttributeValues>("AttributeValues")
                        : new AttributeValues(),
                PropertyCharacteristics =
                    this.scenarioContext.Keys.Contains("PropertyCharacteristic")
                        ? this.scenarioContext.Get<List<PropertyCharacteristic>>("PropertyCharacteristic")
                        : new List<PropertyCharacteristic>()
            };

            dataContext.Properties.Add(property);
            dataContext.CommitAndClose();
            this.scenarioContext.Set(property, "Property");
        }

        [Given(@"Property with (.*) division and (.*) type is defined")]
        public void GetDivisionAndType(string division, string propertyType)
        {
            KnightFrankContext dataContext = DatabaseExtensions.GetDataContext();

            Guid propertyTypeId = dataContext.PropertyTypes.Single(i => i.Code.Equals("House")).Id;
            Guid divisionId =
                dataContext.EnumTypeItems.Single(i => i.EnumType.Code.Equals("Division") && i.Code.Equals("Residential")).Id;

            dataContext.Close();
            this.scenarioContext.Set(propertyTypeId, "PropertyTypeId");
            this.scenarioContext.Set(divisionId, "DivisionId");
        }

        [Given(@"Property attributes details are defined")]
        public void SetPropertyAttributes(Table table)
        {
            var attributeValues = table.CreateInstance<AttributeValues>();
            this.scenarioContext.Set(attributeValues, "AttributeValues");
        }

        [Given(@"Property characteristics are defined")]
        public void SetPropertyCharacterstics()
        {
            var propertyTypeId = this.scenarioContext.Get<Guid>("PropertyTypeId");
            KnightFrankContext dataContext = DatabaseExtensions.GetDataContext();
            Guid countryId = dataContext.Countries.Single(x => x.IsoCode == "GB").Id;

            var list = new List<Guid>();

            foreach (
                Guid id in
                    dataContext.CharacteristicGroupUsages.Where(
                        x => x.PropertyTypeId.Equals(propertyTypeId) && x.CountryId.Equals(countryId)).Select(x => x.Id).ToList())
            {
                list.AddRange(dataContext.CharacteristicGroupItems.Where(x => x.CharacteristicGroupUsageId.Equals(id))
                                         .Select(x => x.CharacteristicId));
            }

            dataContext.Close();
            this.scenarioContext.Set(list.Select(id => new PropertyCharacteristic
            {
                CharacteristicId = id,
                Text = "Comment"
            }).ToList(), "PropertyCharacteristic");
        }

        [Given(@"Property ownership is defined")]
        public void CreateOwnership(Table table)
        {
            List<Ownership> ownerships = table.CreateSet<Ownership>().ToList();
            KnightFrankContext dataContext = DatabaseExtensions.GetDataContext();

            foreach (Ownership ownership in ownerships)
            {
                ownership.PropertyId = this.scenarioContext.Get<Property>("Property").Id;
                ownership.OwnershipTypeId =
                    dataContext.EnumTypeItems.Single(i => i.EnumType.Code.Equals("OwnershipType") && i.Code.Equals("Freeholder"))
                               .Id;
                ownership.Contacts = this.scenarioContext.Get<List<Contact>>("ContactsList");
            }

            dataContext.Ownerships.AddRange(ownerships);
            dataContext.CommitAndClose();
        }

        [Given(@"Property (.*) activity is defined")]
        public void CreateActivity(string activityType)
        {
            KnightFrankContext dataContext = DatabaseExtensions.GetDataContext();

            Guid activityTypeId = dataContext.ActivityTypes.Single(i => i.Code.Equals(activityType)).Id;
            Guid activityStatusId =
                dataContext.EnumTypeItems.Single(
                    i => i.EnumType.Code.Equals("ActivityStatus") && i.Code.Equals("PreAppraisal")).Id;
            Guid propertyId = this.scenarioContext.Get<Property>("Property").Id;

            var activity = new Activity
            {
                PropertyId = propertyId,
                ActivityTypeId = activityTypeId,
                ActivityStatusId = activityStatusId,
                CreatedDate = DateTime.UtcNow,
                LastModifiedDate = DateTime.UtcNow,
                Contacts = this.scenarioContext.Get<List<Contact>>("ContactsList")
            };

            dataContext.Activities.Add(activity);
            dataContext.CommitAndClose();
            this.scenarioContext.Set(activity, "Activity");
        }
    }
}
