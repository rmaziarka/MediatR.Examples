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
        private readonly ElementLocator activityDetails = new ElementLocator(Locator.CssSelector, ".offer-view-activity .ng-binding");
        private readonly ElementLocator activity = new ElementLocator(Locator.CssSelector, ".offer-view-activity .card-body");
        private readonly ElementLocator vendorSolicitor = new ElementLocator(Locator.CssSelector, "#vendorSolicitor .ng-binding");
        // Requirement
        private readonly ElementLocator requirementDetails = new ElementLocator(Locator.CssSelector, ".offer-view-requirement .ng-binding");
        private readonly ElementLocator requirement = new ElementLocator(Locator.CssSelector, ".offer-view-requirement .card-body");
        private readonly ElementLocator requirementActions = new ElementLocator(Locator.CssSelector, ".offer-view-requirement .card-menu-button");
        private readonly ElementLocator openRequirement = new ElementLocator(Locator.CssSelector, ".offer-view-requirement [action *= 'navigateToRequirement'] li");
        private readonly ElementLocator applicantSolicitor = new ElementLocator(Locator.CssSelector, "#applicantSolicitor .ng-binding");
        // Messages
        private readonly ElementLocator successMessage = new ElementLocator(Locator.CssSelector, ".alert-success {0}");
        private readonly ElementLocator messageText = new ElementLocator(Locator.CssSelector, ".growl-message");
        // Progress summary
        private readonly ElementLocator mortgageStatus = new ElementLocator(Locator.Id, "offer-mortgage-status");
        private readonly ElementLocator mortgageSurveyStatus = new ElementLocator(Locator.Id, "offer-mortgage-survey-status");
        private readonly ElementLocator additionalSurveyStatus = new ElementLocator(Locator.Id, "offer-additional-survey-status");
        private readonly ElementLocator searchStatus = new ElementLocator(Locator.Id, "offer-search-status");
        private readonly ElementLocator enquiries = new ElementLocator(Locator.Id, "offer-enquiries");
        private readonly ElementLocator contractApproved = new ElementLocator(Locator.CssSelector, "#contact-approved .status-yes");
        // Mortgage details
        private readonly ElementLocator mortgageLoanToValue = new ElementLocator(Locator.CssSelector, "#mortgage-loan-to-value .ng-binding");
        private readonly ElementLocator broker = new ElementLocator(Locator.CssSelector, "#broker .ng-binding");
        private readonly ElementLocator lender = new ElementLocator(Locator.CssSelector, "#lender .ng-binding");
        private readonly ElementLocator mortgageSurveyDate = new ElementLocator(Locator.Id, "mortgage-survey-date");
        private readonly ElementLocator mortgageSurveyor = new ElementLocator(Locator.CssSelector, "#surveyor .ng-binding");
        // Additional survey
        private readonly ElementLocator additionalSurveyDate = new ElementLocator(Locator.Id, "additional-survey-date");
        private readonly ElementLocator additionalSurveyor = new ElementLocator(Locator.CssSelector, "#additionalSurveyor .ng-binding");
        // Other details
        private readonly ElementLocator comment = new ElementLocator(Locator.Id, "offer-progress-comment");
        // Upward chain
        private readonly ElementLocator addUpwardChain = new ElementLocator(Locator.CssSelector, "offer-chains-control [ng-click *= 'addChain']");
        private readonly ElementLocator upwardChains = new ElementLocator(Locator.CssSelector, "offer-chains-list card[item = 'chain'] .card");
        private readonly ElementLocator upwardChainDetails = new ElementLocator(Locator.CssSelector, "offer-chains-list card[item = 'chain']:nth-of-type({0}) .card .ng-binding");
        private readonly ElementLocator upwardChainStatuses = new ElementLocator(Locator.CssSelector, "offer-chains-list card[item = 'chain']:nth-of-type({0}) .card-info span");
        private readonly ElementLocator upwardChainActions = new ElementLocator(Locator.CssSelector, "offer-chains-list card[item = 'chain']:nth-of-type({0}) .card-action");
        private readonly ElementLocator editUpwardChain = new ElementLocator(Locator.CssSelector, "offer-chains-list card[item = 'chain']:nth-of-type({0}) .card-action [type = 'edit'] li");
        private readonly ElementLocator property = new ElementLocator(Locator.CssSelector, "offer-chains-list address-form-view .ng-binding");

        public ViewOfferPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public ActivityPreviewPage ActivityPreview => new ActivityPreviewPage(this.DriverContext);

        public CreateChainTransactionPage ChainTransaction => new CreateChainTransactionPage(this.DriverContext);

        public ChainTransactionPreviewPage ChainTransactionPreview => new ChainTransactionPreviewPage(this.DriverContext);

        public List<string> OfferDetails => this.Driver.GetElements(this.details).Select(el => el.Text).ToList();

        public int ActivityNumber => this.Driver.GetElements(this.activity).Count;

        public int RequirementNumber => this.Driver.GetElements(this.requirement).Count;

        public string RequirementDetails => this.Driver.GetElement(this.requirementDetails).Text;

        public string SuccessMessage => this.Driver.GetElement(this.successMessage.Format(this.messageText.Value)).Text;

        public List<string> OfferHeader
            => new List<string> { this.Driver.GetElement(this.status).Text, this.Driver.GetElement(this.title).Text };

        public string MortgageStatus => this.Driver.GetElement(this.mortgageStatus).Text;

        public string MortgageSurveyStatus => this.Driver.GetElement(this.mortgageSurveyStatus).Text;

        public string AdditionalSurveyStatus => this.Driver.GetElement(this.additionalSurveyStatus).Text;

        public string SearchStatus => this.Driver.GetElement(this.searchStatus).Text;

        public string Enquiries => this.Driver.GetElement(this.enquiries).Text;

        public string MortgageLoanToValue => this.Driver.GetElement(this.mortgageLoanToValue).Text;

        public List<string> Broker => this.Driver.GetElements(this.broker).Select(el => el.Text).ToList();

        public List<string> Lender => this.Driver.GetElements(this.lender).Select(el => el.Text).ToList();

        public string MortgageSurveyDate => this.Driver.GetElement(this.mortgageSurveyDate).Text;

        public List<string> MortgageSurveyor => this.Driver.GetElements(this.mortgageSurveyor).Select(el => el.Text).ToList();

        public string AdditionalSurveyDate => this.Driver.GetElement(this.additionalSurveyDate).Text;

        public List<string> AdditionalSurveyor => this.Driver.GetElements(this.additionalSurveyor).Select(el => el.Text).ToList();

        public string Comment => this.Driver.GetElement(this.comment).Text;

        public List<string> VendorSolicitor => this.Driver.GetElements(this.vendorSolicitor).Select(el => el.Text).ToList();

        public List<string> ApplicantSolicitor => this.Driver.GetElements(this.applicantSolicitor).Select(el => el.Text).ToList();

        public int UpwardChainNumber => this.Driver.GetElements(this.upwardChains).Count;

        public string PropertyDetails
            => this.Driver.GetElements(this.property).Select(el => el.Text).ToList().Aggregate((i, j) => i + " " + j);

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
            this.Driver.WaitUntilElementIsNoLongerFound(this.successMessage.Format(string.Empty),
                TimeSpan.FromSeconds(6).TotalSeconds);
            return this;
        }

        public bool IsSuccessMessageDisplayed()
        {
            return this.Driver.IsElementPresent(this.successMessage.Format(string.Empty), BaseConfiguration.MediumTimeout);
        }

        public bool IsContractApproved()
        {
            return this.Driver.IsElementPresent(this.contractApproved, BaseConfiguration.ShortTimeout);
        }

        public string GetActivityDetails()
        {
            List<string> list =
                this.Driver.GetElements(this.activityDetails, element => element.Enabled)
                    .Select(el => el.GetTextContent())
                    .ToList();
            return string.Join(" ", list).Trim();
        }

        public ViewOfferPage AddUpwardChain()
        {
            this.Driver.Click(this.addUpwardChain);
            return this;
        }

        public bool CheckIfAddUpwardChainButtonNotPresent()
        {
            return !this.Driver.IsElementPresent(this.addUpwardChain, BaseConfiguration.ShortTimeout);
        }

        public ViewOfferPage OpenChainPreview(string position)
        {
            this.Driver.Click(this.upwardChainDetails.Format(position));
            return this;
        }

        public string GetUpwardChainDetails(int position)
        {
            return this.Driver.GetElement(this.upwardChainDetails.Format(position)).Text;
        }

        public List<string> GetUpwardChainStatuses(int position)
        {
            return
                this.Driver.GetElements(this.upwardChainStatuses.Format(position)).Select(el => el.GetAttribute("title")).ToList();
        }

        public ViewOfferPage OpenChainActions(int position)
        {
            this.Driver.Click(this.upwardChainActions.Format(position));
            return this;
        }

        public ViewOfferPage EditChain(int position)
        {
            this.Driver.Click(this.editUpwardChain.Format(position));
            return this;
        }
    }

    internal class OfferProgressSummary
    {
        public string MortgageStatus { get; set; }

        public string MortgageSurveyStatus { get; set; }

        public string AdditionalSurveyStatus { get; set; }

        public string SearchStatus { get; set; }

        public string Enquiries { get; set; }

        public bool ContractApproved { get; set; }
    }

    internal class OfferMortgageDetails
    {
        public string MortgageLoanToValue { get; set; }

        public string Broker { get; set; }

        public string BrokerCompany { get; set; }

        public string Lender { get; set; }

        public string LenderCompany { get; set; }

        public string Surveyor { get; set; }

        public string SurveyorCompany { get; set; }

        public string MortgageSurveyDate { get; set; }
    }

    internal class OfferAdditional
    {
        public string AdditionalSurveyDate { get; set; }

        public string AdditionalSurveyor { get; set; }

        public string AdditionalSurveyorCompany { get; set; }

        public string Comment { get; set; }
    }

    internal class SolicitorsData
    {
        public string Vendor { get; set; }

        public string VendorCompany { get; set; }

        public string Applicant { get; set; }

        public string ApplicantCompany { get; set; }
    }
}
