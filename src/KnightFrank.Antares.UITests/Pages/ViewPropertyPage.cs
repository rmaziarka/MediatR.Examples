namespace KnightFrank.Antares.UITests.Pages
{
    using System.Collections.Generic;
    using System.Linq;

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
        private readonly ElementLocator addOwernship = new ElementLocator(Locator.CssSelector, "card-list[items *= 'ownerships'] button");
        private readonly ElementLocator ownershipContacts = new ElementLocator(Locator.CssSelector, "div[ng-repeat *= 'property.ownerships']:nth-of-type({0}) span[ng-repeat *= 'contact']");
        private readonly ElementLocator ownershipDetails = new ElementLocator(Locator.CssSelector, "div[ng-repeat *= 'property.ownerships']:nth-of-type({0}) small");
        private readonly ElementLocator viewOwnershipDetails = new ElementLocator(Locator.CssSelector, "div[ng-repeat *= 'property.ownerships']:nth-of-type({0}) a[ng-click *= 'showOwnershipView']");
        //locators for property activities area
        private readonly ElementLocator addActivityButton = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator activityDate = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator activityVendor = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator activityStatus = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator detailsLink = new ElementLocator(Locator.Id, string.Empty);      
        

        public CreateActivityPage Activity => new CreateActivityPage(this.DriverContext);

        public OwnershipDetailsPage Ownership => new OwnershipDetailsPage(this.DriverContext);

        public ContactsListPage ContactsList => new ContactsListPage(this.DriverContext);

        public ViewActivityPreviewPage PreviewDetails => new ViewActivityPreviewPage(this.DriverContext);

        public ViewPropertyPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public bool IsAddressDetailsVisible(string propertyNumber)
        {
            return this.Driver.IsElementPresent(this.expectedAddressField.Format(propertyNumber), BaseConfiguration.MediumTimeout);
        }

        public bool IsAddressDetailsNotVisible(string propertyNumber)
        {
            return !this.Driver.IsElementPresent(this.expectedAddressField.Format(propertyNumber), BaseConfiguration.ShortTimeout);
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

        public ViewPropertyPage SetOwnership()
        {
            this.Driver.GetElement(this.addOwernship).Click();
            return this;
        }

        public List<string> GetOwnershipContacts(int position)
        {
            List<string> contacts = this.Driver.GetElements(this.ownershipContacts.Format(position)).Select(c => c.Text.Replace(",", "")).ToList();
            return contacts;
        }

        public string GetOwnershipDetails(int position)
        {
            return this.Driver.GetElement(this.ownershipDetails.Format(position)).Text;
        }

        public void OpenOwnershipDetails(int position)
        {
            this.Driver.GetElement(this.viewOwnershipDetails.Format(position)).Click();
        }

        public ViewPropertyPage ClickDetailsLink()
        {
            this.Driver.GetElement(this.detailsLink).Click();
            return this;
        }
    }
}
