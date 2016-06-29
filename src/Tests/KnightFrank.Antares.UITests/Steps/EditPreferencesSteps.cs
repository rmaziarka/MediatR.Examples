namespace KnightFrank.Antares.UITests.Steps
{
    using System;

    using KnightFrank.Antares.UITests.Pages;

    using Objectivity.Test.Automation.Common;

    using TechTalk.SpecFlow;

    using Xunit;

    [Binding]
    public class EditPreferencesSteps
    {
        private readonly DriverContext driverContext;
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly ScenarioContext scenarioContext;
        private EditPreferencesPage page;

        public EditPreferencesSteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }

            this.scenarioContext = scenarioContext;
            this.driverContext = this.scenarioContext["DriverContext"] as DriverContext;

            if (this.page == null)
            {
                this.page = new EditPreferencesPage(this.driverContext);
            }
        }

        [Given(@"User navigates to edit preferences page")]
        [When(@"User navigates to edit preferences page")]
        public void WhenUserNavigatesToEditPreferencesPage()
        {
            this.page = new EditPreferencesPage(this.driverContext).OpenEditPreferencesPage();
        }

       [When(@"User selects (.*) format")]
       [Given(@"User selects (.*) format")]
        public void WhenUserSelectsFormat(string setSalutationFormat)
        {
            this.page.SelectSalutaionFormat(setSalutationFormat);
        }

        [When(@"User saves preferences")]
        [Then(@"User saves preferences")]
        public void SavePreferences()
        {
            this.page.SavePreferences();
        }

        [Then(@"User salutation is set to (.*)")]
        public void ThenUserSalutationIsSet(string salutation)
        {
            Assert.True(this.page.IsSalutationValue(salutation));
        }
    }
}
