namespace KnightFrank.Antares.UITests.Steps
{
    using System;
    using System.Threading;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.UITests.Pages;

    using Objectivity.Test.Automation.Common;

    using TechTalk.SpecFlow;

    using Xunit;

    [Binding]
    public class SearchPropertySteps
    {
        private readonly DriverContext driverContext;
        private readonly ScenarioContext scenarioContext;
        private SearchPropertyPage page;

        private string property;

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

        [When(@"User searches for added property on search property page")]
        public void SearchProperty()
        {
            var prop = this.scenarioContext.Get<Property>("Property");
            this.property = prop.Address.PropertyNumber + " " + prop.Address.PropertyName + " " + prop.Address.Line2 + " " +
                            prop.Address.Line3 + " " + prop.Address.Postcode;
            this.page.SearchProperty(this.property);
        }

        [When(@"User waits (.*) seconds")]
        public void WhenUserWaitsSeconds(double seconds)
        {
            Thread.Sleep(TimeSpan.FromSeconds(seconds));
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

        [Then(@"Proper address details are displayed on first property on search property page")]
        public void CheckAddressDetails()
        {
            Assert.Equal(this.page.GetAddressDetails(), this.property);
        }

        [Then(@"Ownership details (.*) are displayed on first property on search property page")]
        public void CheckOwnershipDetails(string ownership)
        {
            Assert.Equal(this.page.AddressOwnership, ownership);
        }
    }
}
