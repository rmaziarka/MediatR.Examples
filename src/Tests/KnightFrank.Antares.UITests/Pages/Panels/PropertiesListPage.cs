namespace KnightFrank.Antares.UITests.Pages.Panels
{
    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class PropertiesListPage : ProjectPageBase
    {
        private readonly ElementLocator applyButton = new ElementLocator(Locator.Id, "property-list-card-apply-button");
        private readonly ElementLocator property = new ElementLocator(Locator.XPath, "//property-list-card//card[contains(@id, '{0}')]/div");
        private readonly ElementLocator sidePanelLoader = new ElementLocator(Locator.CssSelector, ".slide-in.side-panel-loading");

        public PropertiesListPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public PropertiesListPage WaitForPropertiesListToLoad()
        {
            this.Driver.WaitUntilElementIsNoLongerFound(this.sidePanelLoader, BaseConfiguration.MediumTimeout);
            this.Driver.WaitForAngularToFinish();
            return this;
        }

        public PropertiesListPage SelectProperty(string text)
        {
            this.Driver.Click(this.property.Format(text));
            return this;
        }

        public PropertiesListPage ApplyProperty()
        {
            this.Driver.Click(this.applyButton);
            return this;
        }
    }
}
