namespace KnightFrank.Antares.UITests.Pages
{
    using System.Collections.Generic;
    using System.Linq;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class ViewActivityPreviewPage : ProjectPageBase
    {
        private readonly ElementLocator creationDate = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator status = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator vendor = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator viewActivityLink = new ElementLocator(Locator.Id, string.Empty);

        public ViewActivityPreviewPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public string GetStatus()
        {
            return this.Driver.GetElement(this.status).Text;
        }

        public string GetCreationDate()
        {
            return this.Driver.GetElement(this.creationDate).Text;
        }

        public List<string> GetVendor()
        {
            return this.Driver.GetElements(this.vendor).Select(element => element.Text).ToList();
        }

        public ViewActivityPreviewPage ClickViewActivity()
        {
            this.Driver.GetElement(this.viewActivityLink).Click();
            return this;
        }
    }
}
