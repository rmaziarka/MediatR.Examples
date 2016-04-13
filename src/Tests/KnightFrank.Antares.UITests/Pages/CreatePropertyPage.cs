namespace KnightFrank.Antares.UITests.Pages
{
    using System.Linq;

    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;
    using Objectivity.Test.Automation.Common.WebElements;

    using OpenQA.Selenium;

    public class CreatePropertyPage : ProjectPageBase
    {
        private readonly ElementLocator propertyType = new ElementLocator(Locator.Id, "type");
        private readonly ElementLocator propertyTypeLink = new ElementLocator(Locator.CssSelector, "a[ng-click *= 'changeDivision']:not([class *= 'ng-hide'])");
        private readonly ElementLocator saveButton = new ElementLocator(Locator.Id, "saveBtn");

        public CreatePropertyPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public AddressTemplatePage AddressTemplate => new AddressTemplatePage(this.DriverContext);

        public CreatePropertyPage OpenCreatePropertyPage()
        {
            new CommonPage(this.DriverContext).NavigateToPage("create property");
            return this;
        }

        public CreatePropertyPage SelectPropertyType(string type)
        {
            var select = this.Driver.GetElement<Select>(this.propertyType);
            IWebElement element = select.SelectElement().Options.Single(o => o.Text.Trim().Equals(type));
            select.SelectByIndex(select.SelectElement().Options.IndexOf(element));
            return this;
        }

        public CreatePropertyPage SelectType(string type)
        {
            switch (type.ToLower())
            {
                case "residential":
                    if (!this.Driver.GetElement(this.propertyTypeLink).Text.ToLower().Contains("commercial"))
                    {
                        this.Driver.GetElement(this.propertyTypeLink).Click();
                    }
                    break;
                case "commercial":
                    if (!this.Driver.GetElement(this.propertyTypeLink).Text.ToLower().Contains("residential"))
                    {
                        this.Driver.GetElement(this.propertyTypeLink).Click();
                    }
                    break;
            }
            this.Driver.WaitForAngularToFinish();
            return this;
        }

        public ViewPropertyPage SaveProperty()
        {
            this.Driver.GetElement(this.saveButton).Click();
            this.Driver.WaitForAngularToFinish();
            return new ViewPropertyPage(this.DriverContext);
        }
    }
}
