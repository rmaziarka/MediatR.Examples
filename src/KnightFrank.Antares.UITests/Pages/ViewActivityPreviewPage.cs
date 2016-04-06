namespace KnightFrank.Antares.UITests.Pages
{
    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class ViewActivityPreviewPage : ProjectPageBase
    {
        private readonly ElementLocator viewActivityLink = new ElementLocator(Locator.Id, string.Empty);

        public ViewActivityPreviewPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public ViewActivityPreviewPage ClickViewActivity()
        {
            this.Driver.GetElement(this.viewActivityLink).Click();
            return this;
        }
    }
}
