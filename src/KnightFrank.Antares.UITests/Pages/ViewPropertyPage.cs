namespace KnightFrank.Antares.UITests.Pages
{
    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class ViewPropertyPage : ProjectPageBase
    {
        //locators for property address area
        private readonly ElementLocator expectedAddressField = new ElementLocator(Locator.XPath, "//address-form-view//span[text()='{0}']");
        private readonly ElementLocator editButton = new ElementLocator(Locator.CssSelector, "button[ng-click*='goToEdit']");

        //locators for property details area
        //
        //locators for property ownership area
        //
        //locators for property activities area
        private readonly ElementLocator addActivityButton = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator activityDate = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator activityVendor = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator activityStatus = new ElementLocator(Locator.Id, string.Empty);
        

        public CreateActivityPage Activity => new CreateActivityPage(this.DriverContext);

        public ViewPropertyPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public bool IsAddressDetailsVisible(string propertyNumber)
        {
            ElementLocator expectedField = this.expectedAddressField.Format(propertyNumber);
            return this.Driver.IsElementPresent(expectedField, BaseConfiguration.MediumTimeout);
        }

        public bool IsAddressDetailsNotVisible(string propertyNumber)
        {
            ElementLocator expectedField = this.expectedAddressField.Format(propertyNumber);
            return !this.Driver.IsElementPresent(expectedField, BaseConfiguration.ShortTimeout);
        }

        public ViewPropertyPage AddActivity()
        {
            this.Driver.GetElement(this.addActivityButton).Click();
            return this;
        }

        public string GetActivityVendor()
        {
            return this.Driver.GetElement(this.activityVendor).Text;
        }

        public string GetActivityStatus()
        {
            return this.Driver.GetElement(this.activityStatus).Text;
        }

        public string GetActivityDate()
        {
            return this.Driver.GetElement(this.activityDate).Text;
        }

        public CreatePropertyPage EditProperty()
        {
            this.Driver.GetElement(this.editButton).Click();
            return new CreatePropertyPage(this.DriverContext);
        }
    }
}
