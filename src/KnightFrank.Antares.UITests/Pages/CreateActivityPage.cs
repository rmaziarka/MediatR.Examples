namespace KnightFrank.Antares.UITests.Pages
{
    using System.Collections.Generic;
    using System.Linq;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class CreateActivityPage : ProjectPageBase
    {
        private readonly ElementLocator status = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator vendor = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator saveButton = new ElementLocator(Locator.Id, string.Empty);

        public CreateActivityPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public string GetActivityStatus()
        {
            return this.Driver.GetElement(this.status).Text;
        }

        public List<string> GetActivityVendor()
        {
            return this.Driver.GetElements(this.vendor).Select(element => element.Text).ToList();
        }

        public CreateActivityPage SaveActivity()
        {
            this.Driver.GetElement(this.saveButton).Click();
            return this;
        }
    }
}
