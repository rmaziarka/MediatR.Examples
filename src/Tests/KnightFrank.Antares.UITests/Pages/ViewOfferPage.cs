namespace KnightFrank.Antares.UITests.Pages
{
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

        public ViewOfferPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public ActivityPreviewPage ActivityPreview => new ActivityPreviewPage(this.DriverContext);

        public List<string> OfferDetails => this.Driver.GetElements(this.details).Select(el => el.Text).ToList();

        public int ActivityNumber => this.Driver.GetElements(this.activity).Count;

        public int RequirementNumber => this.Driver.GetElements(this.requirement).Count;

        public string ActivityDetails => this.Driver.GetElement(this.activityDetails).Text;

        public string RequirementDetails => this.Driver.GetElement(this.requirementDetails).Text;

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

        public List<string> GetOfferHeader()
        {
            return new List<string> { this.Driver.GetElement(this.status).Text, this.Driver.GetElement(this.title).Text };
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
    }
}
