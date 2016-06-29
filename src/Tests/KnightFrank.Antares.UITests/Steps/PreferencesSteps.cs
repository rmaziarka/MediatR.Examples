namespace KnightFrank.Antares.UITests.Steps
{
    using System;

    using KnightFrank.Antares.UITests.Pages;

    using Objectivity.Test.Automation.Common;

    using TechTalk.SpecFlow;

    [Binding]
    public class PreferencesSteps
    {
        private readonly DriverContext driverContext;
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly ScenarioContext scenarioContext;
        private PreferencesPage page;

        public PreferencesSteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }

            this.scenarioContext = scenarioContext;
            this.driverContext = this.scenarioContext["DriverContext"] as DriverContext;

            if (this.page == null)
            {
                this.page = new PreferencesPage(this.driverContext);
            }
        }

        [Given(@"User navigates to preferences page")]
        [When(@"User navigates to preferences page")]
        public void OpenPreferencesPage()
        {
            this.page = new PreferencesPage(this.driverContext).OpenPreferencesPage();
        }

        [Given(@"User selects (.*) format on preferences page")]
        [When(@"User selects (.*) format on preferences page")]
        public void SelectFormat(string setSalutationFormat)
        {
            this.page.SelectSalutaionFormat(setSalutationFormat);
        }

        [When(@"User saves preferences on preferences page")]
        public void SavePreferences()
        {
            this.page.SavePreferences();
        }
    }
}
