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
    public class CreateTenancySteps
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly DriverContext driverContext;
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly ScenarioContext scenarioContext;
        private readonly CreateTenancyPage page;

        public CreateTenancySteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }

            this.scenarioContext = scenarioContext;
            this.driverContext = this.scenarioContext["DriverContext"] as DriverContext;

            if (this.page == null)
            {
                this.page = new CreateTenancyPage(this.driverContext);
            }
        }

        [When(@"Users fills in terms on create tenancy page")]
        public void SetTerms(Table table)
        {
            var terms = table.CreateInstance<TenancyDetails>();
            this.page.SetStartDate(terms.StartDate)
                .SetEndDate(terms.EndDate)
                .SetAgreedRent(terms.AggredRent);
        }

        [When(@"User clicks save button on create tenancy page")]
        public void SaveTenancy()
        {
            this.page.SaveTenancy();
        }

        [Then(@"Create tenancy page should be displayed")]
        public void CheckIfCreateTenancyPresent()
        {
            Assert.True(this.page.IsCreateTenancyPresent());
        }

        [Then(@"Offer activity details on create tenancy page are same as the following")]
        public void CheckOfferActivity(Table table)
        {
            var expectedDetails = table.CreateInstance<OfferData>();
            Assert.Equal(expectedDetails.Details, this.page.GetActivityDetails());
        }

        [Then(@"Offer requirement details on create tenancy page are same as the following")]
        public void CheckOfferRequirement(Table table)
        {
            var expectedDetails = table.CreateInstance<OfferData>();
            Assert.Equal(expectedDetails.Details, this.page.RequirementDetails);
        }

        [Then(@"Tenants are displayed on create tenancy page")]
        public void CheckTenants(Table table)
        {
            {
                List<string> expectedAttendees = table.CreateSet<TenancyDetails>().Select(x => x.Tenants).ToList();
                List<string> actualAttendess = this.page.TenancyTenants;
                expectedAttendees.ShouldBeEquivalentTo(actualAttendess);
            }
        }

        [Then(@"Landlords are displayed on create tenenacy page")]
        public void CheckLandlords(Table table)
        {
            List<string> expectedAttendees = table.CreateSet<TenancyDetails>().Select(x => x.Landlords).ToList();
            List<string> actualAttendess = this.page.TenancyLandlords;
            expectedAttendees.ShouldBeEquivalentTo(actualAttendess);
        }
    }
}
