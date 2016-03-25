namespace KnightFrank.Antares.UITests.Pages
{
    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;
    using Objectivity.Test.Automation.Common.WebElements;

    public class CreatePropertyPage : ProjectPageBase
    {
        private readonly ElementLocator propertyType = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator saveButton = new ElementLocator(Locator.Id, "saveBtn");

        public CreatePropertyPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public CreatePropertyPage OpenAddPropertyPage()
        {
            new CommonPage(this.DriverContext).NavigateToPage("add property");
            return this; ;
        }

        public AddressTemplatePage AddressTemplate => new AddressTemplatePage(this.DriverContext);

        public CreatePropertyPage SelectPropertyType(string type)
        {
            if (!this.Driver.GetElement<Checkbox>(this.propertyType.Format(type)).Selected)
            {
                this.Driver.GetElement<Checkbox>(this.propertyType.Format(type)).TickCheckbox();
            }
            return this;
        }

        public ViewPropertyPage SaveProperty()
        {
            this.Driver.GetElement(this.saveButton).Click();
            return new ViewPropertyPage(this.DriverContext);
        }
    }
}
