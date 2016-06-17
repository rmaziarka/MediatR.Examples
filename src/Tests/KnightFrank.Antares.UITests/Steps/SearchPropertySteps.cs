namespace KnightFrank.Antares.UITests.Steps
{
    using System;

    using KnightFrank.Antares.UITests.Pages;

    using Objectivity.Test.Automation.Common;

    using TechTalk.SpecFlow;

    using Xunit;

    [Binding]
    public class SearchPropertySteps
    {
        private readonly DriverContext driverContext;
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly ScenarioContext scenarioContext;
        private SearchPropertyPage page;

        public SearchPropertySteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }

            this.scenarioContext = scenarioContext;
            this.driverContext = this.scenarioContext["DriverContext"] as DriverContext;

            if (this.page == null)
            {
                this.page = new SearchPropertyPage(this.driverContext);
            }
        }

        [When(@"User navigates to search property page")]
        public void OpenSearchPropertyPage()
        {
            this.page = new SearchPropertyPage(this.driverContext).OpenSearchPropertyPage();
        }

        [When(@"User searches for (.*) on search property page")]
        public void SearchProperty(string property)
        {
            this.page.SearchProperty(property);
        }

        [When(@"User clicks on first property on search property page")]
        public void ClickSearchResult()
        {
            this.page.ClickSearchResult();
        }

        [Then(@"Search form on search property page should be displayed")]
        public void CheckIfSearchPropertyPresent()
        {
            Assert.True(this.page.IsViewSearchFormPresent());
        }

        [Then(@"Address details (.*) are displayed on first property on search property page")]
        public void CheckAddressDetails(string address)
        {
            Assert.Equal(this.page.AddressDetails, address);
        }

        [Then(@"Ownership details (.*) are displayed on first property on search property page")]
        public void CheckOwnershipDetails(string ownership)
        {
            Assert.Equal(this.page.AddressOwnership, ownership);
        }
    }
}
