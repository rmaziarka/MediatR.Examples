namespace KnightFrank.Antares.UITests.Steps
{
    using System;

    using KnightFrank.Antares.UITests.Pages;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class EditActivitySteps
    {
        private readonly ScenarioContext scenarioContext;

        public EditActivitySteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            this.scenarioContext = scenarioContext;
        }

        [When(@"User edits activity details on edit activity page")]
        public void EditActivityDetails(Table table)
        {
            var details = table.CreateInstance<EditActivityDetails>();
            var page = this.scenarioContext.Get<EditActivityPage>("EditActivityPage");
            page.SelectActivityStatus(details.ActivityStatus)
                .SetMarketAppraisalPrice(details.MarketAppraisalPrice)
                .SetRecommendedPrice(details.RecommendedPrice)
                .SetVendorEstimatedPrice(details.VendorEstimatedPrice);

            this.scenarioContext.Set(page.SaveActivity(), "ViewActivityPage");
        }
    }
}
