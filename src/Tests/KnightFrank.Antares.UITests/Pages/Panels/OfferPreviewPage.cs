namespace KnightFrank.Antares.UITests.Pages.Panels
{
    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class OfferPreviewPage : ProjectPageBase
    {
        private readonly ElementLocator detailsLink = new ElementLocator(Locator.CssSelector, ".slide-in side-panel-content > .section-details:first-of-type a");
        private readonly ElementLocator details = new ElementLocator(Locator.CssSelector, ".slide-in #activity-details");
        private readonly ElementLocator status = new ElementLocator(Locator.CssSelector, "#offer-status .ng-binding");
        private readonly ElementLocator offer = new ElementLocator(Locator.Id, "offer-preview-price");
        private readonly ElementLocator offerPerWeek = new ElementLocator(Locator.Id, "offer-price-per-week");
        private readonly ElementLocator offerDate = new ElementLocator(Locator.Id, "offer-preview-date");
        private readonly ElementLocator offerSpecialConditions = new ElementLocator(Locator.Id, "offer-preview-special-conditions");
        private readonly ElementLocator offerNegotiator = new ElementLocator(Locator.Id, "offer-preview-negotiator");
        private readonly ElementLocator offerProposedexchangeDate = new ElementLocator(Locator.Id, "offer-preview-exchange-date");
        private readonly ElementLocator offerProposedCompletionDate = new ElementLocator(Locator.Id, "offer-preview-completion-date");
        private readonly ElementLocator viewLink = new ElementLocator(Locator.CssSelector, ".slide-in #activity-link > a");
        private readonly ElementLocator loadingIndicator = new ElementLocator(Locator.CssSelector, "activity-preview-panel .busy");

        public OfferPreviewPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public string Details => this.Driver.GetElement(this.details).Text;

        public string Status => this.Driver.GetElement(this.status).Text;

        public string Offer => this.Driver.GetElement(this.offer).Text;

        public string OfferPerWeek => this.Driver.GetElement(this.offerPerWeek).Text;

        public string Date => this.Driver.GetElement(this.offerDate).Text;

        public string SpecialConditions => this.Driver.GetElement(this.offerSpecialConditions).Text;

        public string Negotiator => this.Driver.GetElement(this.offerNegotiator).Text;

        public string ProposedexchangeDate => this.Driver.GetElement(this.offerProposedexchangeDate).Text;

        public string ProposedCompletionDate => this.Driver.GetElement(this.offerProposedCompletionDate).Text;

        public OfferPreviewPage ClickViewLink()
        {
            this.Driver.Click(this.viewLink);
            return this;
        }

        public OfferPreviewPage ClickDetailsLink()
        {
            this.Driver.Click(this.detailsLink);
            return this;
        }

        public OfferPreviewPage WaitForDetailsToLoad()
        {
            this.Driver.WaitUntilElementIsNoLongerFound(this.loadingIndicator, BaseConfiguration.MediumTimeout);
            return this;
        }
    }
}
