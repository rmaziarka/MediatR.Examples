using System;
using KnightFrank.Antares.UI.Tests.Pages;
using Objectivity.Test.Automation.Common;
using TechTalk.SpecFlow;
using Xunit;

namespace KnightFrank.Antares.UI.Tests.Steps
{
    [Binding]
    public class GoogleSteps
    {
        private readonly DriverContext driverContext;

        public GoogleSteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null) throw new ArgumentNullException(nameof(scenarioContext));
            var sc = scenarioContext;

            driverContext = sc["DriverContext"] as DriverContext;
        }

        [Given(@"User navigates to google page")]
        public void OpenGooglePage()
        {
            var googlePage = new GooglePage(driverContext).OpenGooglePage();
            ScenarioContext.Current["GooglePage"] = googlePage;
        }

        [When(@"User looks for '(.*)' in google")]
        public void SetSearchCriteria(string criteria)
        {
            ScenarioContext.Current.Get<GooglePage>("GooglePage").SetSearchCriteria(criteria).ClickSearchButton();
        }

        [Then(@"results should be visible")]
        public void CheckResults()
        {
            var googlePage = ScenarioContext.Current.Get<GooglePage>("GooglePage");
            Assert.True(googlePage.CheckIfResultsAreDisplayed(), "Results are not displayed");
        }
    }
}
