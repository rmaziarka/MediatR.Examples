namespace KnightFrank.Antares.UITests.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Company;
    using KnightFrank.Antares.UITests.Pages;

    using Objectivity.Test.Automation.Common;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    using Xunit;

    [Binding]
    public class ViewCompanySteps
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly DriverContext driverContext;

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
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

        [Then(@"View company page is displayed with following details")]
        public void ThenViewCompanyPageIsDisplayedWithFollowingDetails(Table table)
        {
            List<Company> companies = table.CreateSet<Company>().ToList();
            Assert.True(this.page.IsViewCompanyFormPresent());

            string name = this.page.GetCompanyName();
            string websiteUrl = this.page.GetWebsiteUrl();
            string clientCareUrl = this.page.GetClientCareUrl();

            Company company = companies.First();
            name.ShouldBeEquivalentTo(company.Name);
            websiteUrl.ShouldBeEquivalentTo(company.WebsiteUrl);
            clientCareUrl.ShouldBeEquivalentTo(company.ClientCarePageUrl);
        }

		[When(@"User clicks edit company button on view company page")]
		public void EditCompany()
		{
			this.page.EditCompany();
		}
	}
}
