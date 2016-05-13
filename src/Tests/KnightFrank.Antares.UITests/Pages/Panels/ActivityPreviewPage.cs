namespace KnightFrank.Antares.UITests.Pages.Panels
{
    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class ActivityPreviewPage : ProjectPageBase
    {
        private readonly ElementLocator viewActivityLink = new ElementLocator(Locator.CssSelector, "a[ng-click *= 'goToActivityView']");
        private readonly ElementLocator panel = new ElementLocator(Locator.CssSelector, ".side-panel.slide-in");

        public ActivityPreviewPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public ViewActivityPage ClickViewActivity()
        {
            this.Driver.GetElement(this.viewActivityLink).Click();
            return new ViewActivityPage(this.DriverContext);
        }

        public ActivityPreviewPage WaitForPanelToBeVisible()
        {
            this.Driver.WaitForElementToBeDisplayed(this.panel, BaseConfiguration.MediumTimeout);
            return this;
        }
    }
}
