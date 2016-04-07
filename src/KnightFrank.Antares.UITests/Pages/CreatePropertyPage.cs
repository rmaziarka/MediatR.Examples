namespace KnightFrank.Antares.UITests.Pages
{
    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;
    using Objectivity.Test.Automation.Common.WebElements;

    public class CreatePropertyPage : ProjectPageBase
    {
        private readonly ElementLocator propertyType = new ElementLocator(Locator.Id, "type");
        private readonly ElementLocator propertyTypeLink = new ElementLocator(Locator.CssSelector, "a[ng-click *= 'changeDivision']:not([class *= 'ng-hide'])");
        private readonly ElementLocator saveButton = new ElementLocator(Locator.Id, "saveBtn");

        public CreatePropertyPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public AddressTemplatePage AddressTemplate => new AddressTemplatePage(this.DriverContext);

        public CreatePropertyPage OpenAddPropertyPage()
        {
            new CommonPage(this.DriverContext).NavigateToPage("create property");
            return this;
        }

        public CreatePropertyPage SelectPropertyType(string type)
        {
            this.Driver.WaitForAngular();
            this.Driver.GetElement<Select>(this.propertyType).SelectByText(type);
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
            this.Driver.WaitForAngular();
            return this;
        }

        public ViewPropertyPage SaveProperty()
        {
            this.Driver.GetElement(this.saveButton).Click();
            this.Driver.WaitForAngular();
            return new ViewPropertyPage(this.DriverContext);
        }
    }
}
