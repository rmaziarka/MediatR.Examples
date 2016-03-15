namespace KnightFrank.Antares.UITests.Pages
{
    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;
    using Objectivity.Test.Automation.Common.WebElements;

    public class OwnershipDetailsPage : ProjectPageBase
    {
        private readonly ElementLocator ownershipType = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator ownershipState = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator purchasingDate = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator sellingDate = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator buyingPrice = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator sellingPrice = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator saveButton = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator cancelButton = new ElementLocator(Locator.Id, string.Empty);

        public OwnershipDetailsPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public OwnershipDetailsPage SelectOwnershipType(string type)
        {
            this.Driver.GetElement<Select>(this.ownershipType).SelectByValue(type);
            return this;
        }

        public OwnershipDetailsPage SetCurrentOwnership()
        {
            if (!this.Driver.GetElement<Checkbox>(this.ownershipState).Selected)
            {
                this.Driver.GetElement<Checkbox>(this.ownershipState).TickCheckbox();
            }
            return this;
        }

        public OwnershipDetailsPage SetOwnership()
        {
            if (this.Driver.GetElement<Checkbox>(this.ownershipState).Selected)
            {
                this.Driver.GetElement<Checkbox>(this.ownershipState).UntickCheckbox();
            }
            return this;
        }

        public OwnershipDetailsPage SetPurchasingDate(string date)
        {
            this.Driver.SendKeys(this.purchasingDate, date);
            return this;
        }

        public OwnershipDetailsPage SetSellingDate(string date)
        {
            this.Driver.SendKeys(this.sellingDate, date);
            return this;
        }

        public OwnershipDetailsPage SetBuyingPrice(string price)
        {
            this.Driver.SendKeys(this.buyingPrice, price);
            return this;
        }

        public OwnershipDetailsPage SetSellingPrice(string price)
        {
            this.Driver.SendKeys(this.sellingPrice, price);
            return this;
        }

        public void SaveOwnership()
        {
            this.Driver.GetElement(this.saveButton).Click();
        }

        public void CancelCreatingOwnership()
        {
            this.Driver.GetElement(this.cancelButton).Click();
        }
    }
}
