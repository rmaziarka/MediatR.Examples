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
    public class EditCompanySteps
    {
        private readonly DriverContext driverContext;
        private readonly ScenarioContext scenarioContext;
        private EditCompanyPage page;

        public EditCompanySteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }

            this.scenarioContext = scenarioContext;
            this.driverContext = this.scenarioContext["DriverContext"] as DriverContext;

            if (this.page == null)
            {
                this.page = new EditCompanyPage(this.driverContext);
            }
        }

        [When(@"User navigates to edit company page with id")]
        public void OpenEditPropertyPage()
        {
            Guid companyId = this.scenarioContext.Get<Company>("Company").Id;
            this.page = new EditCompanyPage(this.driverContext).OpenEditCompanyPageWithId(companyId.ToString());
        }

        [When(@"User clicks save company button on edit company page")]
        public void SaveCompany()
        {
            this.page.SaveCompany();
        }

        [When(@"User fills in company details on edit company page")]
        public void FillInCompanyData(Table table)
        {
            var details = table.CreateInstance<ViewCompanyPage.CompanyDetails>();

            this.scenarioContext.Set(details.WebsiteUrl, "Url");

            this.page.SetCompanyName(details.Name)
                .SetWebsite(details.WebsiteUrl)
                .SetClientCareUrl(details.ClientCarePageUrl)
                .SelectClientCareStatus(details.ClientCareStatus)
                .SelectCategory(details.CompanyCategory)
                .SelectCompanyType(details.CompanyType)
                .SetDescription(details.Description)
                .SelectRelationshipManager(details.RelationshipManager)
                .SetValid(details.IsValid);
        }

        [When(@"User selects contacts on edit company page")]
        public void SelectContactsForCompany(Table table)
        {
            this.page.AddContactToCompany().WaitForSidePanelToShow();

            IEnumerable<Contact> contacts = table.CreateSet<Contact>();

            foreach (Contact contact in contacts)
            {
                this.page.ContactsList.WaitForContactsListToLoad().SelectContact(contact.FirstName, contact.LastName);
            }
            this.page.ContactsList.SaveContact();
            this.page.WaitForSidePanelToHide();
        }

        [Then(@"List of company contacts should contain following contacts on edit company page")]
        public void CheckContactsList(Table table)
        {
            List<string> contacts =
                table.CreateSet<Contact>().Select(contact => contact.FirstName + " " + contact.LastName).ToList();

            List<string> selectedContacts = this.page.Contacts;

            Assert.Equal(contacts.Count, selectedContacts.Count);
            contacts.ShouldBeEquivalentTo(selectedContacts);
        }
    }
}
