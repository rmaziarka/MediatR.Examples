namespace KnightFrank.Antares.UITests.Steps
{
    using System;
    using System.Linq;

    using KnightFrank.Antares.UITests.Pages;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

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

        [When(@"User clicks property details link on view activity page")]
        public void OpenPreviewPropertyPage()
        {
            this.scenarioContext.Get<ViewActivityPage>("ViewActivityPage").ClickDetailsLink();
        }

        [When(@"User clicks view property link on property preview page")]
        public void OpenViewPropertyPage()
        {
            var page = this.scenarioContext.Get<ViewActivityPage>("ViewActivityPage");
            this.scenarioContext.Set(page.PropertyPreview.WaitForPanelToBeVisible().OpenViewPropertyPage(), "ViewPropertyPage");
        }

        [When(@"User clicks edit button on view activity page")]
        public void EditActivity()
        {
            EditActivityPage page = this.scenarioContext.Get<ViewActivityPage>("ViewActivityPage").EditActivity();
            this.scenarioContext.Set(page, "EditActivityPage");
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

        [Then(@"Activity details on view activty page are following")]
        public void CheckActivityDetails(Table table)
        {
            var details = table.CreateInstance<EditActivityDetails>();
            var page = this.scenarioContext.Get<ViewActivityPage>("ViewActivityPage");

            Assert.Equal(details.ActivityStatus, page.Status);
            Assert.Equal(details.MarketAppraisalPrice.ToString("N2") + " GBP", page.MarketAppraisalPrice);
            Assert.Equal(details.RecommendedPrice.ToString("N2") + " GBP", page.RecommendedPrice);
            Assert.Equal(details.VendorEstimatedPrice.ToString("N2") + " GBP", page.VendorEstimatedPrice);
        }
    }
}
