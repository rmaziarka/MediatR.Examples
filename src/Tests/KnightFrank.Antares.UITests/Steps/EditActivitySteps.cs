namespace KnightFrank.Antares.UITests.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using KnightFrank.Antares.UITests.Pages;

    using Objectivity.Test.Automation.Common;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    using Xunit;

    [Binding]
    public class EditActivitySteps
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly DriverContext driverContext;
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly ScenarioContext scenarioContext;
        private readonly EditActivityPage page;
        private const string Format = "dd-MM-yyyy";

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

        [When(@"User sets (.*) secondary negotiator as lead negotiator on edit activity page")]
        public void SetLeadNegotiatorFromSecondaryNegotiator(int position)
        {
            this.page.SetSecondaryNegotiatorAsLeadNegotiator(position);
        }

        [When(@"User edits secondary negotiators dates on edit activity page")]
        public void UpdateSecondaryNegotiatorsNextCall(Table table)
        {
            List<Negotiator> nextCall = table.CreateSet<Negotiator>().ToList();
            for (var position = 1; position <= nextCall.Count; position ++)
            {
                this.page.EditSecondaryNegotiatorsCallDate(position, nextCall[position - 1].NextCall);
            }
        }

        [Then(@"Lead negotiator next call is set to current date on edit activity page")]
        public void CheckLedNegotiatorNextCall()
        {
            Assert.Equal(DateTime.UtcNow.ToString(Format), this.page.GetLeadNegotiatorNextCall());
        }

        [Then(@"Secondary negotiators next calls are displayed on edit activity page")]
        public void ThenSecondaryNegotiatorsNextCallAreDisplayedOnEditActivityPage(Table table)
        {
            List<Negotiator> expectedNextCall = table.CreateSet<Negotiator>().ToList();
            foreach (Negotiator negotiator in expectedNextCall)
            {
                if (negotiator.NextCall != string.Empty)
                {
                    negotiator.NextCall =
                        DateTime.UtcNow.AddDays(int.Parse(negotiator.NextCall)).ToString(Format);
                }
            }
            List<string> actualNextCall = this.page.SecondaryNegotiatorsNextCalls;
            actualNextCall.Should().Equal(expectedNextCall.Select(el => el.NextCall));
        }
    }
}
