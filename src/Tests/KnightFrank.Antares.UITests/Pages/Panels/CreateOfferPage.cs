namespace KnightFrank.Antares.UITests.Pages.Panels
{
    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;
    using Objectivity.Test.Automation.Common.WebElements;

    public class CreateOfferPage : ProjectPageBase
    {
        private readonly ElementLocator activityDetails = new ElementLocator(Locator.CssSelector, ".slide-in #activity-details");
        private readonly ElementLocator offer = new ElementLocator(Locator.CssSelector, ".slide-in #offer-price");
        private readonly ElementLocator offerPerWeek = new ElementLocator(Locator.CssSelector, ".slide-in #offer-price-per-week");
        private readonly ElementLocator offerDate = new ElementLocator(Locator.CssSelector, ".slide-in #offer-date");
        private readonly ElementLocator proposedCompletionDate = new ElementLocator(Locator.CssSelector, ".slide-in #offer-completion-date");
        private readonly ElementLocator proposedExchangeDate = new ElementLocator(Locator.CssSelector, ".slide-in #offer-exchange-date");
        private readonly ElementLocator saveOffer = new ElementLocator(Locator.CssSelector, ".slide-in button[type = 'submit']");
        private readonly ElementLocator specialConditions = new ElementLocator(Locator.CssSelector, ".slide-in #offer-special-conditions");
        private readonly ElementLocator status = new ElementLocator(Locator.CssSelector, ".slide-in #offer-status");
        private readonly ElementLocator loadingIndicator = new ElementLocator(Locator.CssSelector, "activity-add-panel .busy");

        public CreateOfferPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public string Details => this.Driver.GetElement(this.activityDetails).Text;

        public CreateOfferPage SelectStatus(string text)
        {
            this.Driver.GetElement<Select>(this.status).SelectByText(text);
            return this;
        }

        public CreateOfferPage SetOffer(string text)
        {
            this.Driver.SendKeys(this.offer, text);
            return this;
        }

        public CreateOfferPage SetOfferPerWeek(string text)
        {
            this.Driver.SendKeys(this.offerPerWeek, text);
            return this;
        }

        public CreateOfferPage SetOfferDate(string text)
        {
            this.Driver.SendKeys(this.offerDate, text);
            return this;
        }

        public CreateOfferPage SetSpecialConditions(string text)
        {
            this.Driver.SendKeys(this.specialConditions, text);
            return this;
        }

        public CreateOfferPage SetProposedExchangeDate(string text)
        {
            this.Driver.SendKeys(this.proposedExchangeDate, text);
            return this;
        }

        public CreateOfferPage SetProposedCompletionDate(string text)
        {
            this.Driver.SendKeys(this.proposedCompletionDate, text);
            return this;
        }

        public CreateOfferPage SaveOffer()
        {
            this.Driver.Click(this.saveOffer);
            this.Driver.WaitForAngularToFinish();
            return this;
        }

        public CreateOfferPage WaitForDetailsToLoad()
        {
            this.Driver.WaitUntilElementIsNoLongerFound(this.loadingIndicator, BaseConfiguration.MediumTimeout);
            return this;
        }
    }

    internal class OfferData
    {
        public string Type { get; set; }

        public string Details { get; set; }

        public string Status { get; set; }

        public string Offer { get; set; }

        public string OfferPerWeek { get; set; }

        public string OfferDate { get; set; }

        public string SpecialConditions { get; set; }

        public string ExchangeDate { get; set; }

        public string CompletionDate { get; set; }

        public string Negotiator { get; set; }
    }
}
