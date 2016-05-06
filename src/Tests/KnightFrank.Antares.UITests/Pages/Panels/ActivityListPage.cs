namespace KnightFrank.Antares.UITests.Pages.Panels
{
    using System.Linq;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class ActivityListPage : ProjectPageBase
    {
        private readonly ElementLocator loadingIndicator = new ElementLocator(Locator.CssSelector, "[ng-show *= 'isLoading']");
        private readonly ElementLocator activities = new ElementLocator(Locator.CssSelector, "activities-list label.ng-binding");
        private readonly ElementLocator configureButton = new ElementLocator(Locator.CssSelector, "button[ng-click *= 'showViewingDetailsPanel']");

        public ActivityListPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public ActivityListPage SelectActivity(string activity)
        {
            this.Driver.GetElements(this.activities).First(el => el.Text.Equals(activity)).Click();
            this.Driver.GetElement(this.configureButton).Click();
            return this;
        }

        public ActivityListPage WaitForDetailsToLoad()
        {
            this.Driver.WaitUntilElementIsNoLongerFound(this.loadingIndicator, BaseConfiguration.LongTimeout);
            return this;
        }
    }
}
