namespace KnightFrank.Antares.UITests.Steps
{
    using System;

    using KnightFrank.Antares.UITests.Pages;

    using Objectivity.Test.Automation.Common;

    using TechTalk.SpecFlow;

    [Binding]
    public class CommonSteps
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly DriverContext driverContext;
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly ScenarioContext scenarioContext;
        private readonly CommonPage page;

        public CommonSteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }

            this.scenarioContext = scenarioContext;
            this.driverContext = this.scenarioContext["DriverContext"] as DriverContext;

            if (this.page == null)
            {
                this.page = new CommonPage(this.driverContext);
            }
        }

        [When(@"User goes back to previous page")]
        public void GoBack()
        {
            this.page.GoBack();
        }
    }
}
