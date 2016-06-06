namespace KnightFrank.Antares.UITests.Pages
{
    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;
    using Objectivity.Test.Automation.Common.WebElements;

    public class EditOfferPage : ProjectPageBase
    {
        private readonly ElementLocator offer = new ElementLocator(Locator.Id, "offer-price");
        private readonly ElementLocator offerDate = new ElementLocator(Locator.Id, "offer-date");
        private readonly ElementLocator proposedCompletionDate = new ElementLocator(Locator.Id, "proposed-completion-date");
        private readonly ElementLocator proposedExchangeDate = new ElementLocator(Locator.Id, "offer-proposed-exchange-date");
        private readonly ElementLocator save = new ElementLocator(Locator.Id, "activity-edit-save");
        private readonly ElementLocator specialConditions = new ElementLocator(Locator.Name, "specialConditions");
        private readonly ElementLocator status = new ElementLocator(Locator.Id, "offer-status");

        public EditOfferPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public EditOfferPage SelectStatus(string offerStatus)
        {
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
    }
}
