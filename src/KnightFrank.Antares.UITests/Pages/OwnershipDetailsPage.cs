namespace KnightFrank.Antares.UITests.Pages
{
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;
    using Objectivity.Test.Automation.Common.WebElements;

    public class OwnershipDetailsPage : ProjectPageBase
    {
        private readonly ElementLocator panel = new ElementLocator(Locator.CssSelector, "div.side-panel.slide-in");
        private readonly ElementLocator selectedContacts = new ElementLocator(Locator.CssSelector, "div[ng-repeat *= 'ownership.contacts'] div.ng-binding");
        private readonly ElementLocator ownershipType = new ElementLocator(Locator.Id, "type");
        private readonly ElementLocator ownershipState = new ElementLocator(Locator.CssSelector, "input[ng-model *= 'isCurrentOwner']");
        private readonly ElementLocator purchasingDate = new ElementLocator(Locator.CssSelector, "input[ng-model *= 'purchaseDate']");
        private readonly ElementLocator sellingDate = new ElementLocator(Locator.CssSelector, "input[ng-model *= 'sellDate']");
        private readonly ElementLocator buyingPrice = new ElementLocator(Locator.Id, "buying-price");
        private readonly ElementLocator sellingPrice = new ElementLocator(Locator.Id, "selling-price");
        private readonly ElementLocator saveButton = new ElementLocator(Locator.CssSelector, "button[ng-click *= 'saveOwnership']");
        // Ownership details
        private readonly ElementLocator detailsOwnershipContacts = new ElementLocator(Locator.CssSelector, "ownership-view [ng-repeat *= 'contacts']");
        private readonly ElementLocator detailsOwnershipType = new ElementLocator(Locator.CssSelector, "[translate *= 'OWNERSHIPTYPE'] + div");
        private readonly ElementLocator detailsPurchasingDate = new ElementLocator(Locator.CssSelector, "[translate *= 'PURCHASEDATE'] + div");
        private readonly ElementLocator detailsSellingDate = new ElementLocator(Locator.CssSelector, "[translate *= 'SELLDATE'] + div");
        private readonly ElementLocator detailsBuyingPrice = new ElementLocator(Locator.CssSelector, "[translate *= 'BUYPRICE'] + div");
        private readonly ElementLocator detailsSellingPrice = new ElementLocator(Locator.CssSelector, "[translate *= 'SELLPRICE'] + div");

        public OwnershipDetailsPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public List<string> Contacts => this.Driver.GetElements(this.selectedContacts).Select(c => c.Text).ToList();

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

        public OwnershipDetailsPage SaveOwnership()
        {
            this.Driver.GetElement(this.saveButton).Click();
            return this;
        }

        public List<string> GetOwnershipContacts()
        {
            return this.Driver.GetElements(this.detailsOwnershipContacts).Select(c => c.Text).ToList();
        }

        public string GetOwnershipType()
        {
            return this.Driver.GetElement(this.detailsOwnershipType).Text;
        }

        public string GetPurchasingDate()
        {
            return this.Driver.GetElement(this.detailsPurchasingDate).Text;
        }

        public string GetSellingDate()
        {
            return this.Driver.GetElement(this.detailsSellingDate).Text;
        }

        public string GetBuyingPrice()
        {
            return this.Driver.GetElement(this.detailsBuyingPrice).Text;
        }

        public string GetSellingPrice()
        {
            return this.Driver.GetElement(this.detailsSellingPrice).Text;
        }
    }
}
