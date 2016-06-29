namespace KnightFrank.Antares.UITests.Pages
{
    using System;

    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;
    using Objectivity.Test.Automation.Common.WebElements;

    using OpenQA.Selenium;

    public class EditPreferencesPage : ProjectPageBase
    {
        private readonly ElementLocator salutationFormat = new ElementLocator(Locator.CssSelector, "#salutationFormat select");
        private readonly ElementLocator salutationFormatSelected = new ElementLocator(Locator.CssSelector, "#salutationFormat select option:checked");
        private readonly ElementLocator save = new ElementLocator(Locator.Id, "preferences-save");

        public EditPreferencesPage(DriverContext driverContext) : base(driverContext)
        {
        }

     public EditPreferencesPage OpenEditPreferencesPage()
        {
            new CommonPage(this.DriverContext).NavigateToPage("edit preferences");
            return this;
        }

        public EditPreferencesPage SelectSalutaionFormat(string setSalutationFormat)
        {
          var select =  this.Driver.GetElement<Select>(this.salutationFormat);
            select.SelectByText(setSalutationFormat);
            this.Driver.WaitForAngularToFinish();
            return this;
        }

        public EditPreferencesPage SavePreferences()
        {
            this.Driver.Click(this.save);
            this.Driver.WaitForAngularToFinish();
            return this;
        }

        public bool IsSalutationValue(string salutation)
        {
           IWebElement selectElement = this.Driver.GetElement(this.salutationFormatSelected);
            return String.Compare(selectElement.Text, salutation,StringComparison.CurrentCultureIgnoreCase) == 1;
        }
    }
}
