namespace KnightFrank.Antares.UITests.Pages
{
    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;
    using Objectivity.Test.Automation.Common.WebElements;

    public class PreferencesPage : ProjectPageBase
    {
        private readonly ElementLocator salutationFormat = new ElementLocator(Locator.CssSelector, "#salutationFormat select");
        private readonly ElementLocator save = new ElementLocator(Locator.Id, "preferences-save");

        public PreferencesPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public PreferencesPage OpenPreferencesPage()
        {
            new CommonPage(this.DriverContext).NavigateToPage("preferences");
            return this;
        }

        public PreferencesPage SelectSalutaionFormat(string text)
        {
            this.Driver.GetElement<Select>(this.salutationFormat).SelectByText(text);
            this.Driver.WaitForAngularToFinish();
            return this;
        }

        public PreferencesPage SavePreferences()
        {
            this.Driver.Click(this.save);
            this.Driver.WaitForAngularToFinish();
            return this;
        }
    }
}
