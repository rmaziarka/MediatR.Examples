namespace KnightFrank.Antares.UITests.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Offer;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.UITests.Pages;
    using KnightFrank.Antares.UITests.Pages.Panels;

    using Objectivity.Test.Automation.Common;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    using Xunit;

    [Binding]
    public class ViewOfferSteps
    {
        private readonly DriverContext driverContext;
        private readonly ScenarioContext scenarioContext;
        private ViewOfferPage page;

        public ViewOfferSteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }

            this.scenarioContext = scenarioContext;
            this.driverContext = this.scenarioContext["DriverContext"] as DriverContext;

            if (this.page == null)
            {
                this.page = new ViewOfferPage(this.driverContext);
            }
        }

        [When(@"User navigates to view offer page with id")]
        public void OpenViewActivityPageWithId()
        {
            Guid offerId = this.scenarioContext.Get<Offer>("Offer").Id;
            this.page = new ViewOfferPage(this.driverContext).OpenViewOfferPageWithId(offerId.ToString());
        }

        [When(@"User clicks activity details on view offer page")]
        public void OpenActivityDetails()
        {
            this.page.OpenActivityPreview().WaitForSidePanelToShow();
        }

        [When(@"User clicks view activity link from activity on view offer page")]
        public void OpenViewActivityPage()
        {
            this.page.ActivityPreview.WaitForDetailsToLoad().ClickViewActivity();
        }

        [When(@"User clicks requirement details button on view offer page")]
        public void OpenRequirementDetails()
        {
            this.page.OpenRequirementActions().OpenRequirement();
        }

        [When(@"User clicks edit offer button on view offer page")]
        public void EditOffer()
        {
            this.page.EditOffer();
        }

        [Then(@"View offer page should be displayed")]
        public void CheckIfViewOfferPresent()
        {
            Verify.That(this.driverContext,
                () => Assert.True(this.page.IsViewOfferFormPresent()),
                () => Assert.Equal(this.page.ActivityNumber, 1),
                () => Assert.Equal(this.page.RequirementNumber, 1));
        }

        [Then(@"Sale offer details on view offer page are same as the following")]
        public void CheckSaleOffer(Table table)
        {
            var expectedDetails = table.CreateInstance<OfferData>();
            var offer = this.scenarioContext.Get<OfferData>("Offer");
            List<string> details = this.page.OfferDetails;

            expectedDetails.OfferDate = offer.OfferDate;
            expectedDetails.CompletionDate = offer.CompletionDate;
            expectedDetails.ExchangeDate = offer.ExchangeDate;

            Verify.That(this.driverContext,
                () => Assert.Equal(expectedDetails.Status, details[0]),
                () => Assert.Equal(int.Parse(expectedDetails.Offer).ToString("N0") + " GBP", details[1]),
                () => Assert.Equal(expectedDetails.OfferDate, details[2]),
                () => Assert.Equal(expectedDetails.SpecialConditions, details[3]),
                () => Assert.Equal(expectedDetails.ExchangeDate, details[4]),
                () => Assert.Equal(expectedDetails.CompletionDate, details[5]),
                () => Assert.Equal(expectedDetails.Negotiator, details[6]));
        }

        [Then(@"Letting offer details on view offer page are same as the following")]
        public void CheckLettingOffer(Table table)
        {
            var expectedDetails = table.CreateInstance<OfferData>();
            var offer = this.scenarioContext.Get<OfferData>("Offer");
            List<string> details = this.page.OfferDetails;

            expectedDetails.OfferDate = offer.OfferDate;
            expectedDetails.CompletionDate = offer.CompletionDate;
            expectedDetails.ExchangeDate = offer.ExchangeDate;

            Verify.That(this.driverContext,
                () => Assert.Equal(expectedDetails.Status, details[0]),
                () => Assert.Equal(int.Parse(expectedDetails.OfferPerWeek).ToString("N0") + " GBP / week", details[1]),
                () => Assert.Equal(expectedDetails.OfferDate, details[2]),
                () => Assert.Equal(expectedDetails.SpecialConditions, details[3]),
                () => Assert.Equal(expectedDetails.ExchangeDate, details[4]),
                () => Assert.Equal(expectedDetails.CompletionDate, details[5]),
                () => Assert.Equal(expectedDetails.Negotiator, details[6]));
        }

        [Then(@"Offer header details on view offer page are same as the following")]
        public void CheckOfferHeader(Table table)
        {
            var expectedDetails = table.CreateInstance<OfferData>();
            List<string> details = this.page.OfferHeader;

            Verify.That(this.driverContext,
                () => Assert.Equal(expectedDetails.Details, details[1]),
                () => Assert.Equal(expectedDetails.Status.ToLower(), details[0].ToLower()));
        }

        [Then(@"Offer activity details on view offer page are same as the following")]
        public void CheckOfferActivity(Table table)
        {
            var expectedDetails = table.CreateInstance<OfferData>();

            Verify.That(this.driverContext,
                () => Assert.Equal(expectedDetails.Details, this.page.GetActivityDetails()));
        }

        [Then(@"Offer requirement details on view offer page are same as the following")]
        public void CheckOfferRequirement(Table table)
        {
            var expectedDetails = table.CreateInstance<OfferData>();

            Verify.That(this.driverContext,
                () => Assert.Equal(expectedDetails.Details, this.page.RequirementDetails));
        }

        [Then(@"Activity details on view offer page are same as the following")]
        public void CheckActivityDetailsPanel(Table table)
        {
            var expectedDetails = table.CreateInstance<ActivityDetails>();
            expectedDetails.CreationDate = this.scenarioContext.Get<Activity>("Activity").CreatedDate.ToString("dd-MM-yyyy");
            List<string> details = this.page.ActivityPreview.GetActivityDetails();
            string vendors = this.page.ActivityPreview.Vendors.Aggregate((i, j) => i + ";" + j);

            Verify.That(this.driverContext,
                () => Assert.Equal(expectedDetails.Status, details[0]),
                () => Assert.Equal(expectedDetails.Negotiator, details[1]),
                () => Assert.Equal(expectedDetails.CreationDate, details[2]),
                () => Assert.Equal(expectedDetails.Type, details[3]),
                () => Assert.Equal(expectedDetails.Vendor, vendors));
        }

        [Then(@"Offer updated success message should be displayed")]
        public void CheckIfSuccessMessageDisplayed()
        {
            Verify.That(this.driverContext,
                () => Assert.True(this.page.IsSuccessMessageDisplayed()),
                () => Assert.Equal("Offer successfully saved", this.page.SuccessMessage));
            this.page.WaitForSuccessMessageToHide();
        }

        [Then(@"Offer progress summary details on view offer page are same as the following")]
        public void CheckProgressSummary(Table table)
        {
            var details = table.CreateInstance<OfferProgressSummary>();

            Verify.That(this.driverContext,
                () => Assert.Equal(details.MortgageStatus, this.page.MortgageStatus),
                () => Assert.Equal(details.MortgageSurveyStatus, this.page.MortgageSurveyStatus),
                () => Assert.Equal(details.AdditionalSurveyStatus, this.page.AdditionalSurveyStatus),
                () => Assert.Equal(details.SearchStatus, this.page.SearchStatus),
                () => Assert.Equal(details.Enquiries, this.page.Enquiries),
                () => Assert.Equal(details.ContractApproved, this.page.IsContractApproved()));
        }

        [Then(@"Offer mortgage details details on view offer page are same as the following")]
        public void CheckMortgageDetails(Table table)
        {
            var details = table.CreateInstance<OfferMortgageDetails>();
            details.MortgageSurveyDate = this.scenarioContext.Get<OfferMortgageDetails>("OfferMortgageDetails").MortgageSurveyDate;

            Verify.That(this.driverContext,
                () => Assert.Equal(details.MortgageLoanToValue + "%", this.page.MortgageLoanToValue),
                () => Assert.Equal(details.Broker, this.page.Broker.First()),
                () => Assert.Equal(details.BrokerCompany, this.page.Broker.Last()),
                () => Assert.Equal(details.Lender, this.page.Lender.First()),
                () => Assert.Equal(details.LenderCompany, this.page.Lender.Last()),
                () => Assert.Equal(details.MortgageSurveyDate, this.page.MortgageSurveyDate),
                () => Assert.Equal(details.Surveyor, this.page.MortgageSurveyor.First()),
                () => Assert.Equal(details.SurveyorCompany, this.page.MortgageSurveyor.Last()));
        }

        [Then(@"Offer additional details on view offer page are same as the following")]
        public void CheckAdditionalDetails(Table table)
        {
            var details = table.CreateInstance<OfferAdditional>();
            details.AdditionalSurveyDate = this.scenarioContext.Get<OfferAdditional>("OfferAdditional").AdditionalSurveyDate;

            Verify.That(this.driverContext,
                () => Assert.Equal(details.AdditionalSurveyDate, this.page.AdditionalSurveyDate),
                () => Assert.Equal(details.AdditionalSurveyor, this.page.AdditionalSurveyor.First()),
                () => Assert.Equal(details.AdditionalSurveyorCompany, this.page.AdditionalSurveyor.Last()),
                () => Assert.Equal(details.Comment, this.page.Comment));
        }

        [Then(@"Offer solicitors details on view offer page are same as the following")]
        public void CheckSolicitors(Table table)
        {
            var details = table.CreateInstance<SolicitorsData>();

            Verify.That(this.driverContext,
                () => Assert.Equal(details.Vendor, this.page.VendorSolicitor.First()),
                () => Assert.Equal(details.VendorCompany, this.page.VendorSolicitor.Last()),
                () => Assert.Equal(details.Applicant, this.page.ApplicantSolicitor.First()),
                () => Assert.Equal(details.ApplicantCompany, this.page.ApplicantSolicitor.Last()));
        }
    }
}
