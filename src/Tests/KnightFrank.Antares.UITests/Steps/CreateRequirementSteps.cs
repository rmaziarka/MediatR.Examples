namespace KnightFrank.Antares.UITests.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.UITests.Pages;

    using Objectivity.Test.Automation.Common;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    using Xunit;

    [Binding]
    public class CreateRequirementSteps
    {
        private readonly DriverContext driverContext;
        private readonly ScenarioContext scenarioContext;
        private CreateRequirementPage page;

        public CreateRequirementSteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }

            this.scenarioContext = scenarioContext;
            this.driverContext = this.scenarioContext["DriverContext"] as DriverContext;

            if (this.page == null)
            {
                this.page = new CreateRequirementPage(this.driverContext);
            }
        }

        [When(@"User navigates to create requirement page")]
        public void OpenCreateRequirementPage()
        {
            this.page = new CreateRequirementPage(this.driverContext).OpenCreateRequirementPage();
        }

        [When(@"User fills in location details on create requirement page")]
        public void SetLocationRequirementDetails(Table table)
        {
            var details = table.CreateInstance<Address>();

            this.page.AddressTemplate.SelectPropertyCountry(table.Rows[0]["Country"])
                .SetPropertyCity(details.City)
                .SetPropertyAddressLine2(details.Line2)
                .SetPropertyPostCode(details.Postcode);
        }

        [When(@"User fills in property details on create requirement page")]
        public void SetPropertyRequirementDetails(Table table)
        {
            var details = table.CreateInstance<Requirement>();

            this.page
                .SetPropertyRequirementsNote(details.Description);
        }

        [When(@"User selects contacts on create requirement page")]
        public void SelectContactsForRequirement(Table table)
        {
            this.page.SelectApplicants().WaitForSidePanelToShow();

            IEnumerable<Contact> contacts = table.CreateSet<Contact>();

            foreach (Contact contact in contacts)
            {
                this.page.ContactsList.WaitForContactsListToLoad().SelectContact(contact.FirstName, contact.Surname);
            }
            this.page.ContactsList.SaveContact();
            this.page.WaitForSidePanelToHide();
        }

        [When(@"User clicks save requirement button on create requirement page")]
        public void SaveNewResidentialSalesRequirement()
        {
            this.page.SaveRequirement();
            this.scenarioContext["RequirementDate"] = DateTime.UtcNow;
        }

        [Then(@"List of applicants should contain following contacts")]
        public void CheckApplicantsList(Table table)
        {
            List<string> applicants =
                table.CreateSet<Contact>().Select(contact => contact.FirstName + " " + contact.Surname).ToList();

            List<string> selectedApplicants = this.page.Applicants;

            Assert.Equal(applicants.Count, selectedApplicants.Count);
            applicants.ShouldBeEquivalentTo(selectedApplicants);
        }

        [Then(@"Requirement form on create requirement page should be displayed")]
        public void CheckIfRequirementFormPresent()
        {
            Assert.True(new CreateRequirementPage(this.driverContext).IsRequirementFormPresent());
        }
    }
}
