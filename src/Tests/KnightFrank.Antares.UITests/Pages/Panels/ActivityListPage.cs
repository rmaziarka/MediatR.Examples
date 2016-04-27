namespace KnightFrank.Antares.UITests.Pages.Panels
{
    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class ActivityListPage : ProjectPageBase
    {
        private readonly ElementLocator activity = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator nextButton = new ElementLocator(Locator.Id, string.Empty);

        public ActivityListPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public ActivityListPage SelectActivity()
        {
            this.Driver.GetElement(this.activity).Click();
            this.Driver.GetElement(this.nextButton).Click();
            return this;
        }
    }
}
