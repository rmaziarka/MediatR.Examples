namespace KnightFrank.Antares.UITests.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Company;
    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.UITests.Pages;

    using Objectivity.Test.Automation.Common;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    using Xunit;

    [Binding]
    public class CreateCompanySteps
    {
        private readonly DriverContext driverContext;
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly ScenarioContext scenarioContext;
        private CreateCompanyPage page;

        public CreateCompanySteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }

            this.scenarioContext = scenarioContext;
            this.driverContext = this.scenarioContext["DriverContext"] as DriverContext;

            if (this.page == null)
            {
                this.page = new CreateCompanyPage(this.driverContext);
            }
        }

        [When(@"User navigates to create company page")]
        public void OpenCreateCompanyPage()
        {
            this.page = new CreateCompanyPage(this.driverContext).OpenCreateCompanyPage();
        }

        [When(@"User fills in company details on create company page")]
        public void FillInCompanyData(Table table)
        {
            var details = table.CreateInstance<Company>();
            this.page.SetCompanyName(details.Name);
        }

        [When(@"User clicks save company button on create company page")]
        public void SaveCompany()
        {
            this.page.SaveCompany();
        }

        [When(@"User selects contacts on create company page")]
        public void SelectContactsForCompany(Table table)
        {
            this.page.SelectContact().WaitForSidePanelToShow();

            IEnumerable<Contact> contacts = table.CreateSet<Contact>();

            foreach (Contact contact in contacts)
            {
                this.page.ContactsList.WaitForContactsListToLoad().SelectContact(contact.FirstName, contact.Surname);
            }
            this.page.ContactsList.SaveContact();
            this.page.WaitForSidePanelToHide();
        }

        [Then(@"List of company contacts should contain following contacts")]
        public void CheckContactsList(Table table)
        {
            List<string> contacts =
                table.CreateSet<Contact>().Select(contact => contact.FirstName + " " + contact.Surname).ToList();

            List<string> selectedContacts = this.page.Contacts;

            Assert.Equal(contacts.Count, selectedContacts.Count);
            contacts.ShouldBeEquivalentTo(selectedContacts);
        }

        [Then(@"Company form on create company page should be diaplyed")]
        public void CheckIfCompanyFormDisplayed()
        {
            Assert.True(this.page.IsCompanyFormPresent());
        }

        [Then(@"New company should be created")]
        public void CheckIfCompanyCreated()
        {
            //TODO implement check if contact was created
        }
    }
}
