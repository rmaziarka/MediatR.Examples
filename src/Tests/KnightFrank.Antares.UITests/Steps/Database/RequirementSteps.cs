namespace KnightFrank.Antares.UITests.Steps.Database
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal;
    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Domain.Common.Enums;
    using KnightFrank.Antares.UITests.Pages;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    using RequirementType = KnightFrank.Antares.Domain.Common.Enums.RequirementType;

    [Binding]
    public class RequirementSteps
    {
        private readonly KnightFrankContext dataContext;
        private readonly ScenarioContext scenarioContext;

        public RequirementSteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }

            this.scenarioContext = scenarioContext;
            this.dataContext = this.scenarioContext.Get<KnightFrankContext>("DataContext");
        }

        [Given(@"Requirement for GB is created in database")]
        [When(@"Requirement for GB is created in database")]
        public void CreateRequirementInDb(Table table)
        {
            var details = table.CreateInstance<RequirementData>();

            RequirementType requirementType = details.Type.Equals("Residential Sale")
                ? RequirementType.ResidentialSale
                : RequirementType.ResidentialLetting;

            // Get country and address form id
            Guid countryId = this.dataContext.Countries.Single(x => x.IsoCode.Equals("GB")).Id;
            Guid enumTypeId =
                this.dataContext.EnumTypeItems.Single(
                    e => e.EnumType.Code.Equals(nameof(EntityType)) && e.Code.Equals(nameof(EntityType.Requirement))).Id;
            Guid addressFormId = this.dataContext.AddressFormEntityTypes.Single(
                afe => afe.AddressForm.CountryId.Equals(countryId) && afe.EnumTypeItemId.Equals(enumTypeId)).AddressFormId;
            Guid requirementTypeId = this.dataContext.RequirementTypes.Single(rt => rt.Code.Equals(requirementType.ToString())).Id;
            
            var requirement = new Requirement
            {
                RequirementTypeId = requirementTypeId,
                CreateDate = DateTime.UtcNow,
                Contacts = this.scenarioContext.Get<List<Contact>>("ContactsList"),
                Address = new Address
                {
                    Line2 = "56 Belsize Park",
                    Postcode = "NW3 4EH",
                    City = "London",
                    AddressFormId = addressFormId,
                    CountryId = countryId
                },
                Description = details.Description
            };

            if (requirementType.Equals(RequirementType.ResidentialLetting))
            {
                requirement.RentMin = 1000;
                requirement.RentMax = 2000;
            }

            this.dataContext.Requirements.Add(requirement);
            this.dataContext.SaveChanges();

            this.scenarioContext.Set(requirement, "Requirement");
        }
    }
}
