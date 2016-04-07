namespace KnightFrank.Antares.UITests.Steps
{
    using System;

    using KnightFrank.Antares.UITests.Pages;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    using Xunit;

    [Binding]
    public class CreateActivitySteps
    {
        private readonly ScenarioContext scenarioContext;

        public CreateActivitySteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            this.scenarioContext = scenarioContext;
        }

        [When(@"User clicks save button on activity panel")]
        public void ClickSaveButtonOnActivityPanel()
        {
            this.scenarioContext.Get<ViewPropertyPage>("ViewPropertyPage").Activity.SaveActivity();
        }

        [Then(@"Activity details are set on activity panel")]
        public void CheckActivityDetailsonActivityPanel(Table table)
        {
            var page = this.scenarioContext.Get<ViewPropertyPage>("ViewPropertyPage");
            var details = table.CreateInstance<ActivityDetails>();

            Assert.Equal(details.Vendor, page.Activity.GetActivityVendor());
            Assert.Equal(details.Status, page.Activity.GetActivityStatus());
        }
    }
}
