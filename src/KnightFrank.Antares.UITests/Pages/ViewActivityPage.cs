namespace KnightFrank.Antares.UITests.Pages
{
    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class ViewActivityPage : ProjectPageBase
    {
        private readonly ElementLocator addressElement = new ElementLocator(Locator.XPath, "//card[@id='activity-view-card-property']//span[text()='{0}']");
        private readonly ElementLocator detailsLink = new ElementLocator(Locator.Id, "detailsLink");
        private readonly ElementLocator editButton = new ElementLocator(Locator.CssSelector, "button[ng-click *= 'goToEdit']");
        private readonly ElementLocator marketAppraisalPrice = new ElementLocator(Locator.Id, "activity-view-prices-marketAppraisalPrice");
        private readonly ElementLocator recommendedPrice = new ElementLocator(Locator.Id, "activity-view-prices-recommendedPrice");
        private readonly ElementLocator status = new ElementLocator(Locator.Id, "activity-view-well-activityStatus");
        private readonly ElementLocator vendorEstimatedPrice = new ElementLocator(Locator.Id, "activity-view-prices-vendorEstimatedPrice");

        public ViewActivityPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public string MarketAppraisalPrice => this.Driver.GetElement(this.marketAppraisalPrice).Text;

        public string RecommendedPrice => this.Driver.GetElement(this.recommendedPrice).Text;

        public string VendorEstimatedPrice => this.Driver.GetElement(this.vendorEstimatedPrice).Text;

        public string Status => this.Driver.GetElement(this.status).Text;

        public PropertyPreviewPage PropertyPreview => new PropertyPreviewPage(this.DriverContext);

        public ViewActivityPage ClickDetailsLink()
        {
            this.Driver.GetElement(this.detailsLink).Click();
            return this;
        }

        public bool IsAddressDetailsVisible(string propertyDetail)
        {
            return this.Driver.IsElementPresent(this.addressElement.Format(propertyDetail), BaseConfiguration.MediumTimeout);
        }

        public bool IsAddressDetailsNotVisible(string propertyDetail)
        {
            return !this.Driver.IsElementPresent(this.addressElement.Format(propertyDetail), BaseConfiguration.ShortTimeout);
        }

        public EditActivityPage EditActivity()
        {
            this.Driver.GetElement(this.editButton).Click();
            return new EditActivityPage(this.DriverContext);
        }
    }
}
