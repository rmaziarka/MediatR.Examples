namespace KnightFrank.Antares.UITests.Pages.Panels
{
    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;
    using Objectivity.Test.Automation.Common.WebElements;

    public class CreateActivityPage : ProjectPageBase
    {
        private readonly ElementLocator saveButton = new ElementLocator(Locator.Id, "activity-add-button");
        private readonly ElementLocator status = new ElementLocator(Locator.Id, "status");
        private readonly ElementLocator vendor = new ElementLocator(Locator.CssSelector, "#activity-add-vendors span.ng-binding");
        private readonly ElementLocator panel = new ElementLocator(Locator.CssSelector, ".side-panel.slide-in");

        public CreateActivityPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public string GetActivityStatus()
        {
            return this.Driver.GetElement<Select>(this.status).SelectElement().SelectedOption.Text;
        }

        public string GetActivityVendor()
        {
            return this.Driver.GetElement(this.vendor).Text;
        }

        public CreateActivityPage SaveActivity()
        {
            this.Driver.GetElement(this.saveButton).Click();
            return this;
        }

        public CreateActivityPage WaitForActivityPanelToHide()
        {
            this.Driver.WaitUntilElementIsNoLongerFound(this.panel, BaseConfiguration.MediumTimeout);
            return this;
        }
    }

    internal class ActivityDetails
    {
        public string Vendor { get; set; }

        public string Status { get; set; }
    }
}
