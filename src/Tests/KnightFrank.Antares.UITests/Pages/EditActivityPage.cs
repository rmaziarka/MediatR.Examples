namespace KnightFrank.Antares.UITests.Pages
{
    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;
    using Objectivity.Test.Automation.Common.WebElements;

    public class EditActivityPage : ProjectPageBase
    {
        private readonly ElementLocator marketAppraisalPrice = new ElementLocator(Locator.Id, "market-appraisal-price");
        private readonly ElementLocator recommendedPrice = new ElementLocator(Locator.Id, "recommended-price");
        private readonly ElementLocator saveButton = new ElementLocator(Locator.Id, "activity-edit-save");
        private readonly ElementLocator status = new ElementLocator(Locator.CssSelector, "#activityStatus > select");
        private readonly ElementLocator vendorEstimatedPrice = new ElementLocator(Locator.Id, "vendor-estimated-price");
        // Locators for negotiators
        private readonly ElementLocator editLeadNegotiator = new ElementLocator(Locator.Id, "lead-edit-btn");
        private readonly ElementLocator searchLeadNegotator = new ElementLocator(Locator.CssSelector, "#lead-search input");
        private readonly ElementLocator addSecondaryNegotiator = new ElementLocator(Locator.CssSelector, "#card-list-negotiators button:not([ng-click *= 'cancel'])");
        private readonly ElementLocator searchSecondaryNegotiator = new ElementLocator(Locator.CssSelector, "#secondary-search input");
        private readonly ElementLocator cancelSecondaryNegotiator = new ElementLocator(Locator.CssSelector, "#card-list-negotiators [ng-click *= 'cancel']");
        private readonly ElementLocator secondaryNegotiatorActions = new ElementLocator(Locator.CssSelector, "#activity-edit-negotiators card-list-item:nth-of-type({0}) .card-menu-button");
        private readonly ElementLocator deleteSecondaryNegotiator = new ElementLocator(Locator.CssSelector, "#activity-edit-negotiators card-list-item:nth-of-type({0}) [action *= 'deleteSecondaryNegotiator']");
        private readonly ElementLocator setSecondaryNegotiatorAsLead = new ElementLocator(Locator.CssSelector, "#activity-edit-negotiators card-list-item:nth-of-type({0}) [action *= 'switchToLeadNegotiator']");
        private readonly ElementLocator negotiator = new ElementLocator(Locator.XPath, "//section[@id = 'activity-edit-negotiators']//span[contains(., '{0}')]");

        public EditActivityPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public EditActivityPage SelectActivityStatus(string newStatus)
        {
            this.Driver.GetElement<Select>(this.status).SelectByText(newStatus);
            return this;
        }

        public EditActivityPage SetMarketAppraisalPrice(int price)
        {
            this.Driver.SendKeys(this.marketAppraisalPrice, price.ToString());
            return this;
        }

        public EditActivityPage SetRecommendedPrice(int price)
        {
            this.Driver.SendKeys(this.recommendedPrice, price.ToString());
            return this;
        }

        public EditActivityPage SetVendorEstimatedPrice(int price)
        {
            this.Driver.SendKeys(this.vendorEstimatedPrice, price.ToString());
            return this;
        }

        public ViewActivityPage SaveActivity()
        {
            this.Driver.GetElement(this.saveButton).Click();
            this.Driver.WaitForAngularToFinish();
            return new ViewActivityPage(this.DriverContext);
        }

        public EditActivityPage EditLeadNegotiator(string leadNegotiator)
        {
            this.Driver.GetElement(this.editLeadNegotiator).Click();
            this.Driver.SendKeys(this.searchLeadNegotator, leadNegotiator);
            this.Driver.WaitForElementToBeDisplayed(this.negotiator.Format(leadNegotiator), BaseConfiguration.MediumTimeout);
            this.Driver.GetElement(this.negotiator.Format(leadNegotiator)).Click();
            return this;
        }

        public EditActivityPage AddSecondaryNegotiator(Negotiator secondaryNegotiator)
        {
            this.Driver.GetElement(this.addSecondaryNegotiator).Click();
            this.Driver.SendKeys(this.searchSecondaryNegotiator, secondaryNegotiator.Name);
            this.Driver.WaitForElementToBeDisplayed(this.negotiator.Format(secondaryNegotiator.Name), BaseConfiguration.MediumTimeout);
            this.Driver.GetElement(this.negotiator.Format(secondaryNegotiator.Name)).Click();
            this.Driver.GetElement(this.cancelSecondaryNegotiator).Click();
            return this;
        }

        public EditActivityPage RemoveSecondaryNegotiator(int position)
        {
            this.Driver.GetElement(this.secondaryNegotiatorActions.Format(position)).Click();
            this.Driver.GetElement(this.deleteSecondaryNegotiator.Format(position)).Click();
            return this;
        }

        public EditActivityPage SetSecondaryNegotiatorAsLeadNegotiator(int position)
        {
            this.Driver.GetElement(this.secondaryNegotiatorActions.Format(position)).Click();
            this.Driver.GetElement(this.setSecondaryNegotiatorAsLead.Format(position)).Click();
            return this;
        }
    }

    internal class EditActivityDetails
    {
        public string ActivityStatus { get; set; }

        public int MarketAppraisalPrice { get; set; }

        public int RecommendedPrice { get; set; }

        public int VendorEstimatedPrice { get; set; }
    }

    public class Negotiator
    {
        public string Name { get; set; }
    }
}
