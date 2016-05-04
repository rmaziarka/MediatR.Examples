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
        private readonly ScenarioContext scenarioContext;

        public CreateCompanySteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            this.scenarioContext = scenarioContext;

            this.driverContext = this.scenarioContext["DriverContext"] as DriverContext;
        }

        [When(@"User navigates to create company page")]
        public void OpenCreateCompanyPage()
        {
            CreateCompanyPage page = new CreateCompanyPage(this.driverContext).OpenCreateCompanyPage();
            this.scenarioContext["CreateCompanyPage"] = page;
        }

        [When(@"User fills in company details on create company page")]
        public void FillInCompanyData(Table table)
        {
            var details = table.CreateInstance<Company>();
            this.scenarioContext.Get<CreateCompanyPage>("CreateCompanyPage").SetCompanyName(details.Name);
        }

        [When(@"User clicks save button on create company page")]
        public void SaveCompany()
        {
            this.scenarioContext.Get<CreateCompanyPage>("CreateCompanyPage").SaveCompany();
        }

        [When(@"User selects contacts on create company page")]
        public void SelectContactsForCompany(Table table)
        {
            var page = this.scenarioContext.Get<CreateCompanyPage>("CreateCompanyPage");
            page.AddContactToCompany();

            IEnumerable<Contact> contacts = table.CreateSet<Contact>();

            foreach (Contact contact in contacts)
            {
                page.ContactsList.WaitForContactsListToLoad().SelectContact(contact.FirstName, contact.Surname);
            }
            page.ContactsList.SaveContact().WaitForContactListToHide();
        }

        [Then(@"list of company contacts should contain following contacts")]
        public void CheckContactsList(Table table)
        {
            var page = this.scenarioContext.Get<CreateCompanyPage>("CreateCompanyPage");

            List<string> contacts =
                table.CreateSet<Contact>().Select(contact => contact.FirstName + " " + contact.Surname).ToList();

            List<string> selectedContacts = page.Contacts;

            Assert.Equal(contacts.Count, selectedContacts.Count);
            contacts.Should().BeEquivalentTo(selectedContacts);
        }

        [Then(@"New company should be created")]
        public void CheckIfCompanyCreated()
        {
            //TODO implement check if contact was created
        }
    }
}
