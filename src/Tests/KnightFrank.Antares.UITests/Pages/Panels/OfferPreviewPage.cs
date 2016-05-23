namespace KnightFrank.Antares.UITests.Pages.Panels
{
    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class OfferPreviewPage : ProjectPageBase
    {
        private readonly ElementLocator details = new ElementLocator(Locator.Id, "offer-preview-activity-details");
        private readonly ElementLocator status = new ElementLocator(Locator.Id, "offer-preview-status");
        private readonly ElementLocator offer = new ElementLocator(Locator.Id, "offer-preview-price");
        private readonly ElementLocator offerDate = new ElementLocator(Locator.Id, "offer-preview-date");
        private readonly ElementLocator offerSpecialConditions = new ElementLocator(Locator.Id, "offer-preview-special-conditions");
        private readonly ElementLocator offerNegotiator = new ElementLocator(Locator.Id, "offer-preview-negotiator");
        private readonly ElementLocator offerProposedexchangeDate = new ElementLocator(Locator.Id, "offer-preview-exchange-date");
        private readonly ElementLocator offerProposedCompletionDate = new ElementLocator(Locator.Id, "offer-preview-completion-date");
        private readonly ElementLocator editOffer = new ElementLocator(Locator.CssSelector, ".slide-in button[ng-click *= 'showEditOfferPreviewPanel']");
        private readonly ElementLocator viewActivityLink = new ElementLocator(Locator.CssSelector, ".slide-in #activity-link > a");
        
        public OfferPreviewPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public string Details => this.Driver.GetElement(this.details).Text;

        public string Status => this.Driver.GetElement(this.status).Text;

        public string Offer => this.Driver.GetElement(this.offer).Text;

        public string Date => this.Driver.GetElement(this.offerDate).Text;

        public string SpecialConditions => this.Driver.GetElement(this.offerSpecialConditions).Text;

        public string Negotiator => this.Driver.GetElement(this.offerNegotiator).Text;

        public string ProposedexchangeDate => this.Driver.GetElement(this.offerProposedexchangeDate).Text;

        public string ProposedCompletionDate => this.Driver.GetElement(this.offerProposedCompletionDate).Text;

        public OfferPreviewPage EditOffer()
        {
            this.Driver.GetElement(this.editOffer).Click();
            return this;
        }

        public OfferPreviewPage ClickViewLink()
        {
            this.Driver.GetElement(this.viewActivityLink).Click();
            return this;
        }
    }
}
