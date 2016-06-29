namespace KnightFrank.Antares.UITests.Pages.Panels
{
    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;
    using Objectivity.Test.Automation.Common.WebElements;

    public class CreateOfferPage : ProjectPageBase
    {
        private readonly ElementLocator activityDetails = new ElementLocator(Locator.CssSelector, ".slide-in #offer-add-activity-details");
        private readonly ElementLocator offer = new ElementLocator(Locator.CssSelector, ".slide-in #offer-price");
        private readonly ElementLocator offerPerWeek = new ElementLocator(Locator.CssSelector, string.Empty);
        private readonly ElementLocator offerDate = new ElementLocator(Locator.CssSelector, ".slide-in #offer-date");
        private readonly ElementLocator proposedCompletionDate = new ElementLocator(Locator.CssSelector, ".slide-in #proposed-completion-date");
        private readonly ElementLocator proposedExchangeDate = new ElementLocator(Locator.CssSelector, ".slide-in #offer-proposed-exchange-date");
        private readonly ElementLocator saveOffer = new ElementLocator(Locator.CssSelector, ".slide-in button[ng-click *= 'save']");
        private readonly ElementLocator specialConditions = new ElementLocator(Locator.CssSelector, ".slide-in [name = 'specialConditions']");
        private readonly ElementLocator status = new ElementLocator(Locator.CssSelector, ".slide-in #offer-status");
        private readonly ElementLocator viewLink = new ElementLocator(Locator.CssSelector, ".slide-in div:nth-of-type(1) a");

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

        public CreateOfferPage ClickViewLink()
        {
            this.Driver.Click(this.viewLink);
            return this;
        }
    }

    internal class OfferData
    {
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
