namespace KnightFrank.Antares.UITests.Pages
{
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.UITests.Extensions;
    using KnightFrank.Antares.UITests.Pages.Panels;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;
    using Objectivity.Test.Automation.Common.WebElements;

    public class EditOfferPage : ProjectPageBase
    {
        private readonly ElementLocator panel = new ElementLocator(Locator.CssSelector, ".side-panel.slide-in");
        private readonly ElementLocator save = new ElementLocator(Locator.Id, "offer-edit-save");
        private readonly ElementLocator status = new ElementLocator(Locator.Id, "offer-status");
        // Vendor
        private readonly ElementLocator activity = new ElementLocator(Locator.CssSelector, "#section-vendor .requirement-view-offers .card-body");
        private readonly ElementLocator vendorSolicitor = new ElementLocator(Locator.Id, "vendorSolicitor-edit");
        // Applicant
        private readonly ElementLocator applicantSolicitor = new ElementLocator(Locator.Id, "applicantSolicitor-edit");
        // Basic information
        private readonly ElementLocator offer = new ElementLocator(Locator.Id, "offer-price");
        private readonly ElementLocator offerDate = new ElementLocator(Locator.Id, "offer-date");
        private readonly ElementLocator specialConditions = new ElementLocator(Locator.Id, "offer-special-conditions");
        private readonly ElementLocator proposedExchangeDate = new ElementLocator(Locator.Id, "offer-exchange-date");
        private readonly ElementLocator proposedCompletionDate = new ElementLocator(Locator.Id, "offer-completion-date");
        // Progress summary
        private readonly ElementLocator mortgageStatus = new ElementLocator(Locator.Id, "offer-mortgage-status");
        private readonly ElementLocator mortgageSurveyStatus = new ElementLocator(Locator.Id, "offer-mortgage-survey-status");
        private readonly ElementLocator additionalSurveyStatus = new ElementLocator(Locator.Id, "offer-additional-survey-status");
        private readonly ElementLocator searchStatus = new ElementLocator(Locator.Id, "offer-search-status");
        private readonly ElementLocator enquiries = new ElementLocator(Locator.Id, "offer-enquiries");
        private readonly ElementLocator contractApproved = new ElementLocator(Locator.CssSelector, "#contact-approved [value = 'true']");
        private readonly ElementLocator contractNotApproved = new ElementLocator(Locator.CssSelector, "#contact-approved [value = 'false']");
        // Mortgage details
        private readonly ElementLocator mortgageLoanToValue = new ElementLocator(Locator.Id, "mortgage-loan-to-value");
        private readonly ElementLocator editBroker = new ElementLocator(Locator.Id, "broker-edit");
        private readonly ElementLocator broker = new ElementLocator(Locator.CssSelector, "#broker .ng-binding");
        private readonly ElementLocator editLender = new ElementLocator(Locator.Id, "lender-edit");
        private readonly ElementLocator lender = new ElementLocator(Locator.CssSelector, "#lender .ng-binding");
        private readonly ElementLocator mortgageSurveyDate = new ElementLocator(Locator.Id, "offer-mortgage-survey-date");
        private readonly ElementLocator mortgageEditSurveyor = new ElementLocator(Locator.Id, "surveyor-edit");
        private readonly ElementLocator mortgageSurveyor = new ElementLocator(Locator.CssSelector, "#surveyor .ng-binding");
        // Additional survey
        private readonly ElementLocator additionalSurveyDate = new ElementLocator(Locator.Id, "offer-additional-survey-date");
        private readonly ElementLocator editAdditionalSurveyor = new ElementLocator(Locator.Id, "additionalSurveyor-edit");
        private readonly ElementLocator additionalSurveyor = new ElementLocator(Locator.CssSelector, "#additionalSurveyor .ng-binding");
        // Other details
        private readonly ElementLocator comment = new ElementLocator(Locator.Id, "offer-progress-comment");

        public EditOfferPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public SelectableContactsListPage ContactsList => new SelectableContactsListPage(this.DriverContext);

        public List<string> Broker => this.Driver.GetElements(this.broker).Select(el => el.Text).ToList();
        
        public List<string> Lender => this.Driver.GetElements(this.lender).Select(el => el.Text).ToList();

        public List<string> MortgageSurveyor => this.Driver.GetElements(this.mortgageSurveyor).Select(el => el.Text).ToList();

        public List<string> AdditionalSurveyor => this.Driver.GetElements(this.additionalSurveyor).Select(el => el.Text).ToList();

        public EditOfferPage OpenEditOfferPageWithId(string id)
        {
            new CommonPage(this.DriverContext).NavigateToPageWithId("edit offer", id);
            return this;
        }

        public EditOfferPage SelectStatus(string offerStatus)
        {
            this.Driver.WaitForAngularToFinish();
            this.Driver.GetElement<Select>(this.status).SelectByText(offerStatus);
            return this;
        }

        public EditOfferPage SetOffer(string price)
        {
            this.Driver.SendKeys(this.offer, price);
            return this;
        }

        public EditOfferPage SetOfferDate(string date)
        {
            this.Driver.SendKeys(this.offerDate, date);
            return this;
        }

        public EditOfferPage SetSpecialConditions(string text)
        {
            this.Driver.SendKeys(this.specialConditions, text);
            return this;
        }

        public EditOfferPage SetProposedExchangeDate(string date)
        {
            this.Driver.SendKeys(this.proposedExchangeDate, date);
            return this;
        }

        public EditOfferPage SetProposedCompletionDate(string date)
        {
            this.Driver.SendKeys(this.proposedCompletionDate, date);
            return this;
        }

        public EditOfferPage SaveOffer()
        {
            this.Driver.Click(this.save);
            return this;
        }

        public EditOfferPage OpenActivityPreview()
        {
            this.Driver.Click(this.activity);
            return this;
        }

        public EditOfferPage WaitForSidePanelToShow()
        {
            this.Driver.WaitForElementToBeDisplayed(this.panel, BaseConfiguration.MediumTimeout);
            return this;
        }

        public EditOfferPage WaitForSidePanelToHide()
        {
            this.Driver.WaitUntilElementIsNoLongerFound(this.panel, BaseConfiguration.MediumTimeout);
            return this;
        }

        public EditOfferPage SelectMortgageStatus(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                this.Driver.GetElement<Select>(this.mortgageStatus).SelectByText(text);
            }
            return this;
        }

        public EditOfferPage SelectMortgageSurveyStatus(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                this.Driver.GetElement<Select>(this.mortgageSurveyStatus).SelectByText(text);
            }
            return this;
        }

        public EditOfferPage SelectAdditionalSurveyStatus(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                this.Driver.GetElement<Select>(this.additionalSurveyStatus).SelectByText(text);
            }
            return this;
        }

        public EditOfferPage SelectSearchStatus(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                this.Driver.GetElement<Select>(this.searchStatus).SelectByText(text);
            }
            return this;
        }

        public EditOfferPage SelectEnquiries(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                this.Driver.GetElement<Select>(this.enquiries).SelectByText(text);
            }
            return this;
        }

        public EditOfferPage ApproveContract()
        {
            this.Driver.Click(this.contractApproved);
            return this;
        }

        public EditOfferPage DisapproveContract()
        {
            this.Driver.Click(this.contractNotApproved);
            return this;
        }

        public EditOfferPage SetMortgageLoanToValue(string text)
        {
            this.Driver.SendKeys(this.mortgageLoanToValue, text);
            return this;
        }

        public EditOfferPage EditBroker()
        {
            this.Driver.Click(this.editBroker);
            return this;
        }

        public EditOfferPage EditLender()
        {
            this.Driver.Click(this.editLender);
            return this;
        }

        public EditOfferPage SetMortgageSurveyDate(string date)
        {
            this.Driver.SendKeys(this.mortgageSurveyDate, date);
            return this;
        }

        public EditOfferPage EditMortgageSurveyor()
        {
            this.Driver.Click(this.mortgageEditSurveyor);
            return this;
        }

        public EditOfferPage SetAdditionalSurveyDate(string date)
        {
            this.Driver.SendKeys(this.additionalSurveyDate, date);
            return this;
        }

        public EditOfferPage EditAdditionalSurveyor()
        {
            this.Driver.Click(this.editAdditionalSurveyor);
            return this;
        }

        public EditOfferPage SetComment(string text)
        {
            this.Driver.SendKeys(this.comment, text);
            return this;
        }

        public EditOfferPage EditVendorSolicitor()
        {
            this.Driver.Click(this.vendorSolicitor);
            return this;
        }

        public EditOfferPage EditApplicantSolicitor()
        {
            this.Driver.Click(this.applicantSolicitor);
            return this;
        }
    }
}
