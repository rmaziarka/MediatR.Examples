namespace KnightFrank.Antares.UITests.Pages.Panels
{
    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class ViewingDetailsPage : ProjectPageBase
    {
        private readonly ElementLocator editViewing = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator panel = new ElementLocator(Locator.Id, string.Empty);

        public ViewingDetailsPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public CreateViewingPage EditViewing()
        {
            this.Driver.GetElement(this.editViewing).Click();
            return new CreateViewingPage(this.DriverContext);
        }

        public ViewingDetailsPage WaitForPanelToBeVisible()
        {
            this.Driver.WaitForElementToBeDisplayed(this.panel, BaseConfiguration.MediumTimeout);
            return this;
        }
    }
}
