namespace KnightFrank.Antares.UITests.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Tenancy;
    using KnightFrank.Antares.UITests.Pages;
    using KnightFrank.Antares.UITests.Pages.Panels;

    using Objectivity.Test.Automation.Common;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    using Xunit;

    [Binding]
    public class ViewTenancySteps
    {
        private readonly DriverContext driverContext;
        private readonly ScenarioContext scenarioContext;
        private ViewTenancyPage page;

        public ViewTenancySteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }

            this.scenarioContext = scenarioContext;
            this.driverContext = this.scenarioContext["DriverContext"] as DriverContext;

            if (this.page == null)
            {
                this.page = new ViewTenancyPage(this.driverContext);
            }
        }

        [When(@"User navigates to view tenancy page with id")]
        public void WhenUserNavigatesToViewTenancyPageWithId()
        {
            Guid tenancyId = this.scenarioContext.Get<Tenancy>("Tenancy").Id;
            this.page = new ViewTenancyPage(this.driverContext).OpenViewTenancytPageWithId(tenancyId.ToString());
        }

        [When(@"User clicks details link from requirement card on view tenancy page")]
        public void OpenRequirementDetailsPage()
        {
            this.page.OpenRequirementsDetails();
        }

        [When(@"User clicks edit button on view tenancy page")]
        public void OpenEditTenancyPage()
        {
            this.page.OpenEditTenancy();
        }

        [Then(@"View tenancy page should be displayed")]
        public void CheckIfCreateTenancyPresent()
        {
            Assert.True(this.page.IsViewTenancyPresent());
        }

        [Then(@"Terms details are following on view tenancy page")]
        public void CheckTermsDetails(Table table)
        {
            var expectedDetails = table.CreateInstance<TenancyDetails>();
            Dictionary<string, string> actualDetails = this.page.GetTerms();
            Verify.That(this.driverContext,
                () => Assert.Equal(expectedDetails.StartDate, actualDetails["startDate"]),
                () => Assert.Equal(expectedDetails.EndDate, actualDetails["endDate"]),
                () => Assert.Equal(int.Parse(expectedDetails.AggredRent).ToString("N0") + " GBP / week", actualDetails["agreedRent"]));
        }

        [Then(@"Offer activity details on view tenancy page are same as the following")]
        public void CheckOfferActivity(Table table)
        {
            var expectedDetails = table.CreateInstance<OfferData>();

            Verify.That(this.driverContext,
                () => Assert.Equal(expectedDetails.Details, this.page.GetActivityDetails()));
        }

        [Then(@"Offer requirement details on view tenancy page are same as the following")]
        public void CheckOfferRequirement(Table table)
        {
            var expectedDetails = table.CreateInstance<OfferData>();

            Verify.That(this.driverContext,
                () => Assert.Equal(expectedDetails.Details, this.page.RequirementDetails));
        }

        [Then(@"Tenants are displayed on view tenancy page")]
        public void CheckTenants(Table table)
        {
            List<string> expectedAttendees = table.CreateSet<TenancyDetails>().Select(x => x.Tenants).ToList();
            List<string> actualAttendess = this.page.TenancyTenants;
            expectedAttendees.ShouldBeEquivalentTo(actualAttendess);
        }

        [Then(@"Landlords are displayed on view tenenacy page")]
        public void CheckLandlords(Table table)
        {
            List<string> expectedAttendees = table.CreateSet<TenancyDetails>().Select(x => x.Landlords).ToList();
            List<string> actualAttendess = this.page.TenancyLandlords;
            expectedAttendees.ShouldBeEquivalentTo(actualAttendess);
        }

        [Then(@"Tenancy title on view tenancy page is following")]
        public void CheckTenancyTitle(Table table)
        {
            var expectedTitle = table.CreateInstance<TenancyDetails>();
            string actualResult = this.page.Title;
            Assert.Equal(expectedTitle.Title, actualResult);
        }
    }
}
