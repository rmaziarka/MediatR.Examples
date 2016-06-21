namespace KnightFrank.Antares.UITests.Pages.Panels
{
    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Types;

    public class PropertyPreviewPage : ProjectPageBase
    {
        private readonly ElementLocator viewPropertyLink = new ElementLocator(Locator.CssSelector, "property-preview-panel a");

        public PropertyPreviewPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public ViewPropertyPage OpenViewPropertyPage()
        {
            this.Driver.Click(this.viewPropertyLink);
            return new ViewPropertyPage(this.DriverContext);
        }
    }
}
