namespace KnightFrank.Antares.UITests.Steps.Database
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal;
    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.UITests.Extensions;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class RequirementSteps
    {
        private readonly ScenarioContext scenarioContext;

        public RequirementSteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }

            this.scenarioContext = scenarioContext;
        }

        [Given(@"Requirement for GB is created in database")]
        public void CreateRequirementInDb(Table table)
        {
            var requirement = table.CreateInstance<Requirement>();
            KnightFrankContext dataContext = DatabaseExtensions.GetDataContext();

            // Get country and address form id
            Guid countryId = dataContext.Countries.Single(x => x.IsoCode == "GB").Id;
            Guid enumTypeId = dataContext.EnumTypeItems.Single(e => e.Code == "Requirement").Id;
            Guid addressFormId =
                dataContext.AddressFormEntityTypes.Single(
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

            dataContext.Requirements.Add(requirement);
            dataContext.CommitAndClose();
            this.scenarioContext.Set(requirement, "Requirement");
        }
    }
}
