namespace KnightFrank.Antares.UITests.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.UITests.Extensions;
    using KnightFrank.Antares.UITests.Pages.Panels;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class ViewOfferPage : ProjectPageBase
    {
        private readonly ElementLocator panel = new ElementLocator(Locator.CssSelector, ".side-panel.slide-in");
        private readonly ElementLocator viewOfferForm = new ElementLocator(Locator.CssSelector, "offer-view > div");
        // Header
        private readonly ElementLocator status = new ElementLocator(Locator.CssSelector, "#view-offer-header .offer-status");
        private readonly ElementLocator title = new ElementLocator(Locator.CssSelector, "#view-offer-header .offer-title");
        private readonly ElementLocator editOffer = new ElementLocator(Locator.CssSelector, "#view-offer-header [ng-click *= 'goToEdit']");
        // Details
        private readonly ElementLocator details = new ElementLocator(Locator.CssSelector, "#section-basic-information .ng-binding");
        // Activity
        private readonly ElementLocator activityDetails = new ElementLocator(Locator.CssSelector, "#section-vendor .requirement-view-offers .ng-binding");
        private readonly ElementLocator activity = new ElementLocator(Locator.CssSelector, "#section-vendor .requirement-view-offers .card-body");
        // Requirement
        private readonly ElementLocator requirementDetails = new ElementLocator(Locator.CssSelector, "#section-applicant .requirement-view-offers .ng-binding");
        private readonly ElementLocator requirement = new ElementLocator(Locator.CssSelector, "#section-applicant .requirement-view-offers .card");
        private readonly ElementLocator requirementActions = new ElementLocator(Locator.CssSelector, "#section-applicant .requirement-view-offers .card-menu-button");
        private readonly ElementLocator openRequirement = new ElementLocator(Locator.CssSelector, "#section-applicant .requirement-view-offers [action *= 'navigateToRequirement']");
        // Messages
        private readonly ElementLocator successMessage = new ElementLocator(Locator.CssSelector, ".alert-success {0}");
        private readonly ElementLocator messageText = new ElementLocator(Locator.CssSelector, ".growl-message");
        // Progress summary
        private readonly ElementLocator mortgageStatus = new ElementLocator(Locator.Id, "offer-mortgage-status");
        private readonly ElementLocator mortgageSurveyStatus = new ElementLocator(Locator.Id, "mortgage-survey-status");
        private readonly ElementLocator additionalSurveyStatus = new ElementLocator(Locator.Id, "additional-survey-status");
        private readonly ElementLocator searchStatus = new ElementLocator(Locator.Id, "search-status");
        private readonly ElementLocator enquiries = new ElementLocator(Locator.Id, "enquiries");
        private readonly ElementLocator contractApproved = new ElementLocator(Locator.CssSelector, "#contract-approved.status-yes");
        // Mortgage details
        private readonly ElementLocator mortgageLoanToValue = new ElementLocator(Locator.CssSelector, ".mortgage-loan-to-value");
        private readonly ElementLocator broker = new ElementLocator(Locator.CssSelector, ".offer-broker .ng-binding");
        private readonly ElementLocator lender = new ElementLocator(Locator.CssSelector, ".offer-lender .ng-binding");
        private readonly ElementLocator mortgageSurveyDate = new ElementLocator(Locator.CssSelector, ".mortgage-survey-date");
        private readonly ElementLocator mortgageSurveyor = new ElementLocator(Locator.CssSelector, ".offer-surveyor .ng-binding");
        // Additional survey
        private readonly ElementLocator additionalSurveyDate = new ElementLocator(Locator.CssSelector, ".additional-survey-date");
        private readonly ElementLocator additionalSurveyor = new ElementLocator(Locator.CssSelector, ".offer-additional-surveyor .ng-binding");
        // Other details
        private readonly ElementLocator comment = new ElementLocator(Locator.CssSelector, "#comment .ng-binding");

        public ViewOfferPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public ActivityPreviewPage ActivityPreview => new ActivityPreviewPage(this.DriverContext);

        public List<string> OfferDetails => this.Driver.GetElements(this.details).Select(el => el.Text).ToList();

        public int ActivityNumber => this.Driver.GetElements(this.activity).Count;

        public int RequirementNumber => this.Driver.GetElements(this.requirement).Count;

        public string ActivityDetails => this.Driver.GetElement(this.activityDetails).Text;

        public string RequirementDetails => this.Driver.GetElement(this.requirementDetails).Text;

        public string SuccessMessage => this.Driver.GetElement(this.successMessage.Format(this.messageText.Value)).Text;

        public List<string> OfferHeader => new List<string> { this.Driver.GetElement(this.status).Text, this.Driver.GetElement(this.title).Text };

        public string MortgageStatus => this.Driver.GetElement(this.mortgageStatus).Text;

        public string MortgageSurveyStatus => this.Driver.GetElement(this.mortgageSurveyStatus).Text;

        public string AdditionalSurveyStatus => this.Driver.GetElement(this.additionalSurveyStatus).Text;

        public string SearchStatus => this.Driver.GetElement(this.searchStatus).Text;

        public string Enquiries => this.Driver.GetElement(this.enquiries).Text;

        public string ContractApproved => this.Driver.GetElement(this.contractApproved).Text;

        public string MortgageLoanToValue => this.Driver.GetElement(this.mortgageLoanToValue).Text;

        public List<string> Brokers => this.Driver.GetElements(this.broker).Select(el => el.Text).ToList();

        public List<string> Lenders => this.Driver.GetElements(this.lender).Select(el => el.Text).ToList();

        public string MortgageSurveyDate => this.Driver.GetElement(this.mortgageSurveyDate).Text;

        public List<string> MortgageSurveyors => this.Driver.GetElements(this.mortgageSurveyor).Select(el => el.Text).ToList();

        public string AdditionalSurveyDate => this.Driver.GetElement(this.additionalSurveyDate).Text;

        public List<string> AdditionalSurveyors => this.Driver.GetElements(this.additionalSurveyor).Select(el => el.Text).ToList();

        public string Comment => this.Driver.GetElement(this.comment).Text;

        public ViewOfferPage OpenViewOfferPageWithId(string id)
        {
            new CommonPage(this.DriverContext).NavigateToPageWithId("view offer", id);
            return this;
        }

        public ViewOfferPage WaitForSidePanelToShow()
        {
            this.Driver.WaitForElementToBeDisplayed(this.panel, BaseConfiguration.MediumTimeout);
            return this;
        }

        public bool IsViewOfferFormPresent()
        {
            return this.Driver.IsElementPresent(this.viewOfferForm, BaseConfiguration.MediumTimeout);
        }

        public ViewOfferPage OpenActivityPreview()
        {
            this.Driver.Click(this.activity);
            return this;
        }

        public ViewOfferPage OpenRequirementActions()
        {
            this.Driver.Click(this.requirementActions);
            return this;
        }

        public ViewOfferPage OpenRequirement()
        {
            this.Driver.Click(this.openRequirement);
            return this;
        }

        public ViewOfferPage EditOffer()
        {
            this.Driver.Click(this.editOffer);
            return this;
        }

        public ViewOfferPage WaitForSuccessMessageToHide()
        {
            this.Driver.WaitUntilElementIsNoLongerFound(this.successMessage.Format(string.Empty), TimeSpan.FromSeconds(6).TotalSeconds);
            return this;
        }

        public bool IsSuccessMessageDisplayed()
        {
            return this.Driver.IsElementPresent(this.successMessage.Format(string.Empty), BaseConfiguration.MediumTimeout);
        }
    }
}
