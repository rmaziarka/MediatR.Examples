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
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly DriverContext driverContext;
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly ScenarioContext scenarioContext;
        private readonly EditCompanyPage page;

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

		[Then(@"Edit company page is displayed with following details")]
		public void ThenEditCompanyPageIsDisplayedWithFollowingDetails(Table table)
		{
			List<Company> companies = table.CreateSet<Company>().ToList();
			Assert.True(this.page.IsEditCompanyFormPresent());

			string name = this.page.GetCompanyName();
			string websiteUrl = this.page.GetWebsiteUrl();
			string clientCareUrl = this.page.GetClientCareUrl();

			Company company = companies.First();
			name.ShouldBeEquivalentTo(company.Name);
			websiteUrl.ShouldBeEquivalentTo(company.WebsiteUrl);
			clientCareUrl.ShouldBeEquivalentTo(company.ClientCarePageUrl);
		}

		[When(@"User fills in company details on edit company page")]
        public void FillInCompanyData(Table table)
        {
            var details = table.CreateInstance<Company>();

			this.scenarioContext.Set(details.WebsiteUrl, "Url");

			this.page.SetCompanyName(details.Name);
            this.page.SetWebsite(details.WebsiteUrl);
            this.page.SetClientCareUrl(details.ClientCarePageUrl);
            this.page.SetClientCareStatus();
		}

		[When(@"User selects contacts on edit company page")]
		public void SelectContactsForCompany(Table table)
		{
			this.page.AddContactToCompany().WaitForSidePanelToShow();

			IEnumerable<Contact> contacts = table.CreateSet<Contact>();

			foreach (Contact contact in contacts)
			{
				this.page.ContactsList.WaitForContactsListToLoad().SelectContact(contact.FirstName, contact.Surname);
			}
			this.page.ContactsList.SaveContact();
			this.page.WaitForSidePanelToHide();
		}

		[Then(@"List of updated company contacts should contain following contacts")]
		public void CheckContactsList(Table table)
		{
			List<string> contacts =
				table.CreateSet<Contact>().Select(contact => contact.FirstName + " " + contact.Surname).ToList();

			List<string> selectedContacts = this.page.Contacts;

			Assert.Equal(contacts.Count, selectedContacts.Count);
			contacts.ShouldBeEquivalentTo(selectedContacts);
		}

		[When(@"User clicks save company button on edit company page")]
        public void SaveCompany()
        {
            this.page.SaveCompany();
        }
    }
}
