namespace KnightFrank.Antares.UITests.Steps
{
    using System;
    using System.Collections.Generic;

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
        }

        [When(@"User clicks save button on edit activity page")]
        public void SaveActivty()
        {
            var page = this.scenarioContext.Get<EditActivityPage>("EditActivityPage");
            this.scenarioContext.Set(page.SaveActivity(), "ViewActivityPage");
        }

        [When(@"User changes lead negotiator to (.*) on edit activity page")]
        public void UpdateLeadNegotiator(string user)
        {
            this.scenarioContext.Get<EditActivityPage>("EditActivityPage").EditLeadNegotiator(user);
        }

        [When(@"User adds secondary negotiators on edit activity page")]
        public void AddSecondaryNegotiators(Table table)
        {
            IEnumerable<Negotiator> secondaryNegotiators = table.CreateSet<Negotiator>();
            foreach (Negotiator element in secondaryNegotiators)
            {
                this.scenarioContext.Get<EditActivityPage>("EditActivityPage").AddSecondaryNegotiator(element);
            }
        }

        [When(@"User removes (.*) secondary negotiator from edit activity page")]
        public void RemoveSecondaryNegotiator(int secondaryNegotiator)
        {
            this.scenarioContext.Get<EditActivityPage>("EditActivityPage").RemoveSecondaryNegotiator(secondaryNegotiator);
        }

    }
}
