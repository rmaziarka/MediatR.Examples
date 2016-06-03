namespace KnightFrank.Antares.UITests.Pages.Panels
{
    using System.Linq;

    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class ActivityListPage : ProjectPageBase
    {
        private readonly ElementLocator loadingIndicator = new ElementLocator(Locator.CssSelector, "[ng-show *= 'isLoading']");
        private readonly ElementLocator activities = new ElementLocator(Locator.CssSelector, "activities-list label.ng-binding");
        private readonly ElementLocator configureButton = new ElementLocator(Locator.CssSelector, "button[ng-click *= 'showViewingAddPanel']");

        public ActivityListPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public ActivityListPage SelectActivity(string activity)
        {
            this.Driver.GetElements(this.activities).Last(el => el.Text.Equals(activity)).Click();
            this.Driver.Click(this.configureButton);
            return this;
        }

        public ActivityListPage WaitForDetailsToLoad()
        {
            this.Driver.WaitUntilElementIsNoLongerFound(this.loadingIndicator, BaseConfiguration.MediumTimeout);
            return this;
        }
    }
}
