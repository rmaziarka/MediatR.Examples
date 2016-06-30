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
    public class ViewCompanySteps
    {
        private readonly DriverContext driverContext;
        private readonly ScenarioContext scenarioContext;
        private ViewCompanyPage page;

        public ViewCompanySteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }

            this.scenarioContext = scenarioContext;
            this.driverContext = this.scenarioContext["DriverContext"] as DriverContext;

            if (this.page == null)
            {
                this.page = new ViewCompanyPage(this.driverContext);
            }
        }

        [When(@"User navigates to view company page with id")]
        public void OpenViewCompanyPageWithId()
        {
            Guid companyId = this.scenarioContext.Get<Company>("Company").Id;
            this.page = new ViewCompanyPage(this.driverContext).OpenViewCompanyPageWithId(companyId.ToString());
        }

        [When(@"User clicks edit company button on view company page")]
        public void EditCompany()
        {
            this.page.EditCompany();
        }

        [Then(@"View company page should be displayed")]
        public void CheckIfViewCompanyDisplayed()
        {
            Assert.True(this.page.IsViewCompanyFormPresent());
        }

        [Then(@"Company should have following details on view company page")]
        public void CheckCompanyDetails(Table table)
        {
            var company = table.CreateInstance<ViewCompanyPage.CompanyDetails>();

            Verify.That(this.driverContext,
                () => Assert.Equal(company.Name, this.page.CompanyName),
                () => Assert.Equal(company.WebsiteUrl, this.page.Website),
                () => Assert.Equal(company.ClientCarePageUrl, this.page.ClientCarePage),
                () => Assert.Equal(company.ClientCareStatus, this.page.ClientCareStatus));
        }

        [Then(@"Company contacts should have following contacts on view company page")]
        public void CheckCompanyContacts(Table table)
        {
            List<string> contacts = table.CreateSet<Contact>().Select(c => c.FirstName + " " + c.LastName).ToList();
            List<string> expectedContacts = this.page.Contacts;

            expectedContacts.Should().Equal(contacts);
        }
    }
}
