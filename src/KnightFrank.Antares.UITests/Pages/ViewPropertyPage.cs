namespace KnightFrank.Antares.UITests.Pages
{
    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class ViewPropertyPage : ProjectPageBase
    {
        private readonly ElementLocator viewPropertyForm = new ElementLocator(Locator.CssSelector, "property-view");
        //locators for property address area
        private readonly ElementLocator expectedAddressField = new ElementLocator(Locator.XPath, "//address-form-view//span[text()='{0}']");
        private readonly ElementLocator editButton = new ElementLocator(Locator.CssSelector, "button[ng-click*='goToEdit']");
        //locators for property details area
        //
        //locators for property ownership area
        private readonly ElementLocator addOwernship = new ElementLocator(Locator.CssSelector, "card-list[show-item-add *= 'showContactList'] button");
        private readonly ElementLocator ownershipContacts = new ElementLocator(Locator.XPath, "//card-list-item[{0}]//span[contains(@ng-repeat, 'contacts')]");
        private readonly ElementLocator ownershipDetails = new ElementLocator(Locator.XPath, "//card-list-item[{0}]//small/span/..");
        //locators for property activities area
        private readonly ElementLocator addActivityButton = new ElementLocator(Locator.CssSelector, "card-list[show-item-add *= 'showActivityAdd'] button");
        private readonly ElementLocator activityDate = new ElementLocator(Locator.CssSelector, "card[item = 'activity'] div.panel-item");
        private readonly ElementLocator activityVendor = new ElementLocator(Locator.CssSelector, "card[item = 'activity'] span");
        private readonly ElementLocator activityStatus = new ElementLocator(Locator.CssSelector, "card[item = 'activity'] small");
        private readonly ElementLocator activityDetailsLink = new ElementLocator(Locator.CssSelector, "#card-list-activities #detailsLink");

        public ViewPropertyPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public CreateActivityPage Activity => new CreateActivityPage(this.DriverContext);

        public OwnershipDetailsPage Ownership => new OwnershipDetailsPage(this.DriverContext);

        public ContactsListPage ContactsList => new ContactsListPage(this.DriverContext);

        public ActivityPreviewPage PreviewDetails => new ActivityPreviewPage(this.DriverContext);

        public bool IsAddressDetailsVisible(string propertyDetail)
        {
            return this.Driver.IsElementPresent(this.expectedAddressField.Format(propertyDetail), BaseConfiguration.MediumTimeout);
        }

        public bool IsAddressDetailsNotVisible(string propertyDetail)
        {
            return !this.Driver.IsElementPresent(this.expectedAddressField.Format(propertyDetail), BaseConfiguration.ShortTimeout);
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
            return this.Driver.GetElement(this.activityDate).Text.Split(' ')[0].Trim();
        }

        public CreatePropertyPage EditProperty()
        {
            this.Driver.GetElement(this.editButton).Click();
            return new CreatePropertyPage(this.DriverContext);
        }

        public ViewPropertyPage SetOwnership()
        {
            this.Driver.GetElement(this.addOwernship).Click();
            return this;
        }

        public string GetOwnershipContact(int position)
        {
            return this.Driver.GetElement(this.ownershipContacts.Format(position)).Text;
        }

        public string GetOwnershipDetails(int position)
        {
            return this.Driver.GetElement(this.ownershipDetails.Format(position)).Text;
        }

        public ViewPropertyPage OpenActivityDetails()
        {
            this.Driver.GetElement(this.activityDetailsLink).Click();
            return this;
        }

        public bool CheckIfViewPropertyPresent()
        {
            this.Driver.WaitForAngular();
            return this.Driver.IsElementPresent(this.viewPropertyForm, BaseConfiguration.ShortTimeout);
        }
    }
}
