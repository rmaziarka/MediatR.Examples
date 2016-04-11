namespace KnightFrank.Antares.UITests.Pages
{
    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;
    using Objectivity.Test.Automation.Common.WebElements;

    public class EditActivityPage : ProjectPageBase
    {
        private readonly ElementLocator marketAppraisalPrice = new ElementLocator(Locator.Id, "market-appraisal-price");
        private readonly ElementLocator recommendedPrice = new ElementLocator(Locator.Id, "recommended-price");
        private readonly ElementLocator saveButton = new ElementLocator(Locator.Id, "activity-edit-save");
        private readonly ElementLocator status = new ElementLocator(Locator.Id, "activityStatus");
        private readonly ElementLocator vendorEstimatedPrice = new ElementLocator(Locator.Id, "vendor-estimated-price");

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
            this.Driver.GetElement(this.marketAppraisalPrice).SendKeys(price.ToString());
            return this;
        }

        public EditActivityPage SetRecommendedPrice(int price)
        {
            this.Driver.GetElement(this.recommendedPrice).SendKeys(price.ToString());
            return this;
        }

        public EditActivityPage SetVendorEstimatedPrice(int price)
        {
            this.Driver.GetElement(this.vendorEstimatedPrice).SendKeys(price.ToString());
            return this;
        }

        public ViewActivityPage SaveActivity()
        {
            this.Driver.GetElement(this.saveButton).Click();
            return new ViewActivityPage(this.DriverContext);
        }
    }

    internal class EditActivityDetails
    {
        public string ActivityStatus { get; set; }

        public int MarketAppraisalPrice { get; set; }

        public int RecommendedPrice { get; set; }

        public int VendorEstimatedPrice { get; set; }
    }
}
