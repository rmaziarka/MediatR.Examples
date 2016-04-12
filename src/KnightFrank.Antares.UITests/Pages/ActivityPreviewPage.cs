namespace KnightFrank.Antares.UITests.Pages
{
    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class ActivityPreviewPage : ProjectPageBase
    {
        private readonly ElementLocator viewActivityLink = new ElementLocator(Locator.CssSelector, "a[ng-click *= 'goToActivityView']");

        public ActivityPreviewPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public ViewActivityPage ClickViewActivity()
        {
            this.Driver.GetElement(this.viewActivityLink).Click();
            return new ViewActivityPage(this.DriverContext);
        }
    }
}
