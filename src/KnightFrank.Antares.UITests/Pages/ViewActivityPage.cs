namespace KnightFrank.Antares.UITests.Pages
{
    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class ViewActivityPage : ProjectPageBase
    {
        private readonly ElementLocator addressElement = new ElementLocator(Locator.XPath, "//card[@id='activity-view-card-property']//span[text()='{0}']");
        private readonly ElementLocator detailsLink = new ElementLocator(Locator.Id, "detailsLink");

        public ViewActivityPage(DriverContext driverContext) : base(driverContext)
        {
        }

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
    }
}
