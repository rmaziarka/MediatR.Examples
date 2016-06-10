namespace KnightFrank.Antares.UITests.Steps.Database
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal;
    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Property;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

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
            var requirement = table.CreateInstance<Requirement>();

            // Get country and address form id
            Guid countryId = this.dataContext.Countries.Single(x => x.IsoCode == "GB").Id;
            Guid enumTypeId = this.dataContext.EnumTypeItems.Single(e => e.Code == "Requirement").Id;
            Guid addressFormId = this.dataContext.AddressFormEntityTypes.Single(
                afe => afe.AddressForm.CountryId == countryId && afe.EnumTypeItemId == enumTypeId).AddressFormId;

            // Set requirement details
            requirement.CreateDate = DateTime.UtcNow;
            requirement.Contacts = this.scenarioContext.Get<List<Contact>>("ContactsList");
            requirement.Address = new Address
            {
                Line2 = "56 Belsize Park",
                Postcode = "NW3 4EH",
                City = "London",
                AddressFormId = addressFormId,
                CountryId = countryId
            };

            this.dataContext.Requirements.Add(requirement);
            this.dataContext.SaveChanges();

            this.scenarioContext.Set(requirement, "Requirement");
        }
    }
}
