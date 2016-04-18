namespace KnightFrank.Antares.UITests.Pages
{
    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class PropertyPreviewPage : ProjectPageBase
    {
        private readonly ElementLocator viewPropertyLink = new ElementLocator(Locator.CssSelector, "a[ng-click *= 'goToPropertyView']");
        private readonly ElementLocator panel = new ElementLocator(Locator.CssSelector, ".side-panel.slide-in");

        public PropertyPreviewPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public ViewPropertyPage OpenViewPropertyPage()
        {
            this.Driver.GetElement(this.viewPropertyLink).Click();
            return new ViewPropertyPage(this.DriverContext);
        }

        public PropertyPreviewPage WaitForPanelToBeVisible()
        {
            this.Driver.WaitForElementToBeDisplayed(this.panel, BaseConfiguration.LongTimeout);
            return this;
        }
    }
}
