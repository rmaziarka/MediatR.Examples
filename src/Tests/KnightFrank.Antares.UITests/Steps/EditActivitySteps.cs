namespace KnightFrank.Antares.UITests.Steps
{
    using System;
    using System.Collections.Generic;

    using KnightFrank.Antares.UITests.Pages;

    using Objectivity.Test.Automation.Common;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class EditActivitySteps
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly DriverContext driverContext;
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly ScenarioContext scenarioContext;
        private readonly EditActivityPage page;

        public EditActivitySteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }

            this.scenarioContext = scenarioContext;
            this.driverContext = this.scenarioContext["DriverContext"] as DriverContext;

            if (this.page == null)
            {
                this.page = new EditActivityPage(this.driverContext);
            }
        }

        [When(@"User edits activity details on edit activity page")]
        public void EditActivityDetails(Table table)
        {
            var details = table.CreateInstance<EditActivityDetails>();

            this.page.SelectActivityStatus(details.ActivityStatus)
                .SetMarketAppraisalPrice(details.MarketAppraisalPrice)
                .SetRecommendedPrice(details.RecommendedPrice)
                .SetVendorEstimatedPrice(details.VendorEstimatedPrice);
        }

        [When(@"User clicks save button on edit activity page")]
        public void SaveActivty()
        {
            this.page.SaveActivity();
        }

        [When(@"User changes lead negotiator to (.*) on edit activity page")]
        public void UpdateLeadNegotiator(string user)
        {
            this.page.EditLeadNegotiator(user);
        }

        [When(@"User adds secondary negotiators on edit activity page")]
        public void AddSecondaryNegotiators(Table table)
        {
            IEnumerable<Negotiator> secondaryNegotiators = table.CreateSet<Negotiator>();
            foreach (Negotiator element in secondaryNegotiators)
            {
                this.page.AddSecondaryNegotiator(element);
            }
        }

        [When(@"User removes (.*) secondary negotiator from edit activity page")]
        public void RemoveSecondaryNegotiator(int secondaryNegotiator)
        {
            this.page.RemoveSecondaryNegotiator(secondaryNegotiator);
        }
    }
}
