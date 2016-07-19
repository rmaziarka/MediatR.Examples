namespace KnightFrank.Antares.UITests.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using KnightFrank.Antares.UITests.Pages;
    using KnightFrank.Antares.UITests.Pages.Panels;

    using Objectivity.Test.Automation.Common;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    using Xunit;

    [Binding]
    public class EditTenancySteps
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly DriverContext driverContext;
        private readonly EditTenancyPage page;
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly ScenarioContext scenarioContext;

        public EditTenancySteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }

            this.scenarioContext = scenarioContext;
            this.driverContext = this.scenarioContext["DriverContext"] as DriverContext;

            if (this.page == null)
            {
                this.page = new EditTenancyPage(this.driverContext);
            }
        }

        [When(@"User updates terms on edit tenancy page")]
        public void SetTerms(Table table)
        {
            var terms = table.CreateInstance<TenancyDetails>();
            this.page.SetStartDate(terms.StartDate)
                .SetEndDate(terms.EndDate)
                .SetAgreedRent(terms.AggredRent);
        }

        [When(@"User clicks save button on edit tenancy page")]
        public void SaveTenancy()
        {
            this.page.SaveTenancy();
        }

        [Then(@"Edit tenancy page should be displayed")]
        public void CheckIfEditTenancyPresent()
        {
            Assert.True(this.page.IsEditTenancyPresent());
        }

        [Then(@"Offer activity details on edit tenancy page are same as the following")]
        public void CheckOfferActivity(Table table)
        {
            var expectedDetails = table.CreateInstance<OfferData>();
            Assert.Equal(expectedDetails.Details, this.page.GetActivityDetails());
        }

        [Then(@"Offer requirement details on edit tenancy page are same as the following")]
        public void CheckOfferRequirement(Table table)
        {
            var expectedDetails = table.CreateInstance<OfferData>();
            Assert.Equal(expectedDetails.Details, this.page.RequirementDetails);
        }
    }
}
