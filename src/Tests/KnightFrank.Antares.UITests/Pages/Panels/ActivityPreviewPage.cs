namespace KnightFrank.Antares.UITests.Pages.Panels
{
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class ActivityPreviewPage : ProjectPageBase
    {
        private readonly ElementLocator creationDate = new ElementLocator(Locator.Id, "activity-preview-created-date");
        private readonly ElementLocator status = new ElementLocator(Locator.Id, "activity-preview-status");
        private readonly ElementLocator type = new ElementLocator(Locator.Id, "activity-preview-type");
        private readonly ElementLocator vendors = new ElementLocator(Locator.CssSelector, "#activity-vendors-preview .ng-binding");
        private readonly ElementLocator landlords = new ElementLocator(Locator.CssSelector, "#activity-landlords-preview .ng-binding");
        private readonly ElementLocator viewActivityLink = new ElementLocator(Locator.CssSelector, ".slide-in a");
        private readonly ElementLocator loadingIndicator = new ElementLocator(Locator.CssSelector, ".slide-in.side-panel-loading");

        public ActivityPreviewPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public List<string> Vendors => this.Driver.GetElements(this.vendors).Select(el => el.Text).ToList();

        public List<string> Landlords => this.Driver.GetElements(this.landlords).Select(el => el.Text).ToList();

        public ViewActivityPage ClickViewActivity()
        {
            this.Driver.Click(this.viewActivityLink);
            return new ViewActivityPage(this.DriverContext);
        }

        public List<string> GetActivityDetails()
        {
            var list = new List<string>
            {
                this.Driver.GetElement(this.status).Text,
                this.Driver.GetElement(this.creationDate).Text,
                this.Driver.GetElement(this.type).Text
            };
            return list;
        }

        public ActivityPreviewPage WaitForDetailsToLoad()
        {
            this.Driver.WaitUntilElementIsNoLongerFound(this.loadingIndicator, BaseConfiguration.MediumTimeout);
            return this;
        }
    }
}
