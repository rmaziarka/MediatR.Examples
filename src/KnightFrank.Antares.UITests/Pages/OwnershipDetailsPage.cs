namespace KnightFrank.Antares.UITests.Pages
{
    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;
    using Objectivity.Test.Automation.Common.WebElements;

    public class OwnershipDetailsPage : ProjectPageBase
    {
        private readonly ElementLocator ownershipState = new ElementLocator(Locator.CssSelector, "input[ng-model *= 'isCurrentOwner']");
        private readonly ElementLocator ownershipType = new ElementLocator(Locator.Id, "type");
        private readonly ElementLocator panel = new ElementLocator(Locator.CssSelector, "div.side-panel.slide-in");
        private readonly ElementLocator purchaseDate = new ElementLocator(Locator.Name, "purchaseDate");
        private readonly ElementLocator purchasePrice = new ElementLocator(Locator.Id, "buying-price");
        private readonly ElementLocator saveButton = new ElementLocator(Locator.CssSelector, "button[ng-click *= 'saveOwnership']");
        private readonly ElementLocator sellDate = new ElementLocator(Locator.Name, "sellDate");
        private readonly ElementLocator sellPrice = new ElementLocator(Locator.Id, "selling-price");

        public OwnershipDetailsPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public OwnershipDetailsPage WaitForOwnershipPanelToHide()
        {
            this.Driver.WaitUntilElementIsNoLongerFound(this.panel, BaseConfiguration.MediumTimeout);
            return this;
        }

        public OwnershipDetailsPage SelectOwnershipType(string type)
        {
            this.Driver.GetElement<Select>(this.ownershipType).SelectByText(type);
            return this;
        }

        public OwnershipDetailsPage SetCurrentOwnership()
        {
            if (!this.Driver.GetElement(this.ownershipState).Selected)
            {
                this.Driver.GetElement(this.ownershipState).Click();
            }
            return this;
        }

        public OwnershipDetailsPage SetOwnership()
        {
            if (this.Driver.GetElement(this.ownershipState).Selected)
            {
                this.Driver.GetElement(this.ownershipState).Click();
            }
            return this;
        }

        public OwnershipDetailsPage SetPurchaseDate(string date)
        {
            this.Driver.SendKeys(this.purchaseDate, date);
            return this;
        }

        public OwnershipDetailsPage SetSellDate(string date)
        {
            this.Driver.SendKeys(this.sellDate, date);
            return this;
        }

        public OwnershipDetailsPage SetPurchasePrice(string price)
        {
            this.Driver.SendKeys(this.purchasePrice, price);
            return this;
        }

        public OwnershipDetailsPage SetSellPrice(string price)
        {
            this.Driver.SendKeys(this.sellPrice, price);
            return this;
        }

        public OwnershipDetailsPage SaveOwnership()
        {
            this.Driver.GetElement(this.saveButton).Click();
            return this;
        }
    }

    internal class OwnershipDetails
    {
        public int Position { get; set; }

        public string ContactName { get; set; }

        public string ContactSurname { get; set; }

        public string Type { get; set; }

        public bool Current { get; set; }

        public string PurchaseDate { get; set; }

        public string SellDate { get; set; }

        public string PurchasePrice { get; set; }

        public string SellPrice { get; set; }
    }
}
