namespace KnightFrank.Antares.UITests.Pages
{
    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class PropertyPreviewPage : ProjectPageBase
    {
        private readonly ElementLocator viewPropertyLink = new ElementLocator(Locator.CssSelector, "a[ng-click *= 'goToPropertyView']");

        public PropertyPreviewPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public ViewPropertyPage OpenViewPropertyPage()
        {
            this.Driver.GetElement(this.viewPropertyLink).Click();
            return new ViewPropertyPage(this.DriverContext);
        }
    }
}
