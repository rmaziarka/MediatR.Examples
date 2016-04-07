namespace KnightFrank.Antares.UITests.Steps
{
    using System;
    using System.Linq;

    using KnightFrank.Antares.UITests.Pages;

    using TechTalk.SpecFlow;

    using Xunit;

    [Binding]
    public class ViewActivitySteps
    {
        private readonly ScenarioContext scenarioContext;

        public ViewActivitySteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            this.scenarioContext = scenarioContext;
        }

        [When(@"User clicks details link on view activity page")]
        public void OpenPreviewPropertyPage()
        {
            this.scenarioContext.Get<ViewActivityPage>("ViewActivityPage").ClickDetailsLink();
        }

        [When(@"User clicks view property link on property preview page")]
        public void OpenViewPropertyPage()
        {
            var page = this.scenarioContext.Get<ViewActivityPage>("ViewActivityPage");
            this.scenarioContext.Set(page.PropertyPreview.OpenViewPropertyPage(), "ViewPropertyPage");
        }

        [Then(@"Address details on view activity page are following")]
        public void CheckViewActivityAddressDetails(Table table)
        {
            var page = this.scenarioContext.Get<ViewActivityPage>("ViewActivityPage");

            foreach (string field in table.Rows.SelectMany(row => row.Values))
            {
                Assert.True(field.Equals(string.Empty)
                    ? page.IsAddressDetailsNotVisible(field)
                    : page.IsAddressDetailsVisible(field));
            }
        }
    }
}
