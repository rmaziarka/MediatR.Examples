namespace KnightFrank.Antares.UITests.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.UITests.Extensions;
    using KnightFrank.Antares.UITests.Pages.Panels;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class ViewPropertyPage : ProjectPageBase
    {
        private readonly ElementLocator panel = new ElementLocator(Locator.CssSelector, ".side-panel.slide-in");
        private readonly ElementLocator viewPropertyForm = new ElementLocator(Locator.CssSelector, "property-view > div");
        // Locators for property address area
        private readonly ElementLocator expectedAddressField = new ElementLocator(Locator.XPath, "//address-form-view//span[text()='{0}']");
        private readonly ElementLocator editButton = new ElementLocator(Locator.CssSelector, "button[ng-click*='goToEdit']");
        // Locators for property details area
        private readonly ElementLocator propertyType = new ElementLocator(Locator.CssSelector, "div[translate = 'PROPERTY.VIEW.TYPE'] ~ div");
        private readonly ElementLocator propertyDetailsLabels = new ElementLocator(Locator.CssSelector, "[ng-repeat *= 'attribute'] div.ng-binding:not([class *= 'ng-hide'])");
        private readonly ElementLocator propertyDetailsValues = new ElementLocator(Locator.CssSelector, "[ng-repeat *= 'attribute'] div.attribute-value div:not([class *= 'ng-hide'])");
        // Locators for property ownership area
        private readonly ElementLocator addOwernship = new ElementLocator(Locator.CssSelector, "#ownership-list button");
        private readonly ElementLocator ownershipContacts = new ElementLocator(Locator.XPath, "//card-list-item[{0}]//span[contains(@ng-repeat, 'contacts')]");
        private readonly ElementLocator ownershipDetails = new ElementLocator(Locator.XPath, "//card-list-item[{0}]//small/span/..");
        // Locators for property activities area        
        private readonly ElementLocator addActivity = new ElementLocator(Locator.CssSelector, "#card-list-activities button");
        private readonly ElementLocator activityDate = new ElementLocator(Locator.CssSelector, "card[item = 'activity'] div.panel-item");
        private readonly ElementLocator activityVendor = new ElementLocator(Locator.CssSelector, "card[item = 'activity'] span");
        private readonly ElementLocator activityType = new ElementLocator(Locator.CssSelector, "card[item = 'activity'] small[id *= 'activity-type']");
        private readonly ElementLocator activityStatus = new ElementLocator(Locator.CssSelector, "card[item = 'activity'] small[id *= 'activity-status']");
        private readonly ElementLocator activityDetailsLink = new ElementLocator(Locator.CssSelector, "#card-list-activities .detailsLink");
        // Locators for characteristics
        private readonly ElementLocator characteristics = new ElementLocator(Locator.CssSelector, ".characteristics li");
        private readonly ElementLocator characteristicName = new ElementLocator(Locator.XPath, "(//characteristic-list-view//span[contains(@class, 'name')])[{0}]");
        private readonly ElementLocator characteristicComment = new ElementLocator(Locator.XPath, "(//characteristic-list-view//span[contains(@class, 'name')])[{0}]/following-sibling::span");
        // Locators for area breakdown
        private readonly ElementLocator addAreaBreakdown = new ElementLocator(Locator.CssSelector, "#card-list-areas button");
        private readonly ElementLocator areaTile = new ElementLocator(Locator.CssSelector, "#card-list-areas card-list-items > div");
        private readonly ElementLocator areaName = new ElementLocator(Locator.CssSelector, "card-list-items > div:nth-of-type({0}) .card-item");
        private readonly ElementLocator areaSize = new ElementLocator(Locator.CssSelector, "card-list-items > div:nth-of-type({0}) .card-info");
        private readonly ElementLocator areaActions = new ElementLocator(Locator.CssSelector, "div[ng-repeat *= 'propertyAreaBreakdown']:nth-of-type({0}) .card-menu-button");
        private readonly ElementLocator areaEdit = new ElementLocator(Locator.CssSelector, "div[ng-repeat *= 'propertyAreaBreakdown']:nth-of-type({0}) [action *= 'showAreaEdit']");
        // Locators for messages
        private readonly ElementLocator successMessage = new ElementLocator(Locator.CssSelector, ".alert-success {0}");
        private readonly ElementLocator messageText = new ElementLocator(Locator.CssSelector, ".growl-message");

        public ViewPropertyPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public CreateActivityPage Activity => new CreateActivityPage(this.DriverContext);

        public OwnershipDetailsPage Ownership => new OwnershipDetailsPage(this.DriverContext);

        public ContactsListPage ContactsList => new ContactsListPage(this.DriverContext);

        public ActivityPreviewPage ActivityPreview => new ActivityPreviewPage(this.DriverContext);

        public CreateAreaPage Area => new CreateAreaPage(this.DriverContext);

        public string PropertyType => this.Driver.GetElement(this.propertyType).Text;

        public string ActivityVendor => this.Driver.GetElement(this.activityVendor).Text;

        public string ActivityStatus => this.Driver.GetElement(this.activityStatus).Text;

        public string ActivityType => this.Driver.GetElement(this.activityType).Text;

        public string SuccessMessage => this.Driver.GetElement(this.successMessage.Format(this.messageText.Value)).Text;

        public string ActivityDate => this.Driver.GetElement(this.activityDate).Text.Split(' ')[0].Trim();

        public ViewPropertyPage OpenViewPropertyPageWithId(string id)
        {
            new CommonPage(this.DriverContext).NavigateToPageWithId("view property", id);
            return this;
        }

        public ViewPropertyPage WaitForSidePanelToShow()
        {
            this.Driver.WaitForElementToBeDisplayed(this.panel, BaseConfiguration.MediumTimeout);
            return this;
        }

        public ViewPropertyPage WaitForSidePanelToHide()
        {
            this.Driver.WaitUntilElementIsNoLongerFound(this.panel, BaseConfiguration.MediumTimeout);
            return this;
        }

        public ViewPropertyPage WaitForSuccessMessageToHide()
        {
            this.Driver.WaitUntilElementIsNoLongerFound(this.successMessage.Format(string.Empty), TimeSpan.FromSeconds(6).TotalSeconds);
            return this;
        }

        public bool IsSuccessMessageDisplayed()
        {
            return this.Driver.IsElementPresent(this.successMessage.Format(string.Empty), BaseConfiguration.MediumTimeout);
        }

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
            this.Driver.Click(this.addActivity);
            return this;
        }

        public CreatePropertyPage EditProperty()
        {
            this.Driver.Click(this.editButton);
            return new CreatePropertyPage(this.DriverContext);
        }

        public ViewPropertyPage SetOwnership()
        {
            this.Driver.Click(this.addOwernship);
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
            this.Driver.Click(this.activityDetailsLink);
            return this;
        }

        public bool IsViewPropertyFormPresent()
        {
            this.Driver.WaitForAngularToFinish();
            return this.Driver.IsElementPresent(this.viewPropertyForm, BaseConfiguration.MediumTimeout);
        }

        public Dictionary<string, string> GetPropertyDetails()
        {
            List<string> keys =
                this.Driver.GetElements(this.propertyDetailsLabels)
                    .Select(el => el.Text.Replace(" ", string.Empty).ToLower())
                    .ToList();
            List<string> values = this.Driver.GetElements(this.propertyDetailsValues).Select(el => el.Text.Trim()).ToList();
            return keys.Zip(values, (key, value) => new { key, value }).ToDictionary(x => x.key, x => x.value);
        }

        public Dictionary<string, string> GetCharacteristics()
        {
            var keys = new List<string>();
            var values = new List<string>();

            int charCount = this.Driver.GetElements(this.characteristics).Count;

            for (var i = 1; i <= charCount; ++i)
            {
                keys.Add(this.Driver.GetElement(this.characteristicName.Format(i)).Text);
                values.Add(this.Driver.IsElementPresent(this.characteristicComment.Format(i), BaseConfiguration.ShortTimeout)
                    ? this.Driver.GetElement(this.characteristicComment.Format(i)).Text
                    : string.Empty);
            }
            return keys.Zip(values, (key, value) => new { key, value }).ToDictionary(x => x.key, x => x.value);
        }

        public ViewPropertyPage CreateAreaBreakdown()
        {
            this.Driver.Click(this.addAreaBreakdown);
            return this;
        }

        public List<PropertyAreaBreakdown> GetAreas()
        {
            var actualResult = new List<PropertyAreaBreakdown>();
            int areasNumber = this.Driver.GetElements(this.areaTile).Count;

            for (var i = 1; i <= areasNumber; i++)
            {
                actualResult.Add(new PropertyAreaBreakdown
                {
                    Name = this.Driver.GetElement(this.areaName.Format(i)).Text,
                    Size =
                        double.Parse(this.Driver.GetElement(this.areaSize.Format(i)).Text.Replace(".00 sq ft", string.Empty).Trim())
                });
            }
            return actualResult;
        }

        public ViewPropertyPage OpenAreaActions(int position)
        {
            this.Driver.Click(this.areaActions.Format(position));
            return this;
        }

        public ViewPropertyPage EditArea(int position)
        {
            this.Driver.Click(this.areaEdit.Format(position));
            return this;
        }
    }

    internal class PropertyDetails
    {
        public string Bedrooms { get; set; }

        public string Receptions { get; set; }

        public string Bathrooms { get; set; }

        public string PropertyArea { get; set; }

        public string LandArea { get; set; }

        public string CarParkingSpaces { get; set; }

        public string GuestRooms { get; set; }

        public string FunctionRooms { get; set; }
    }
}
