namespace KnightFrank.Antares.UITests.Pages.Panels
{
    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;
    using Objectivity.Test.Automation.Common.WebElements;

    public class CreateActivityPage : ProjectPageBase
    {
        private readonly ElementLocator saveButton = new ElementLocator(Locator.Id, "activity-add-button");
        private readonly ElementLocator status = new ElementLocator(Locator.CssSelector, "#status > select");
        private readonly ElementLocator type = new ElementLocator(Locator.CssSelector, "#addActivityForm #type");
        private readonly ElementLocator vendor = new ElementLocator(Locator.CssSelector, "#activity-add-vendors span.ng-binding");

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
            this.Driver.Click(this.saveButton);
            return this;
        }

        public CreateActivityPage SelectActivityType(string activityType)
        {
            this.Driver.GetElement<Select>(this.type).SelectByText(activityType);
            return this;
        }

        public CreateActivityPage SelectActivityStatus(string activityStatus)
        {
            this.Driver.GetElement<Select>(this.status).SelectByText(activityStatus);
            return this;
        }
    }

    internal class ActivityDetails
    {
        public string Vendor { get; set; }

        public string Status { get; set; }

        public string Type { get; set; }

        public string Negotiator { get; set; }

        public string CreationDate { get; set; }
    }
}
