namespace KnightFrank.Antares.UITests.Pages
{
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.UITests.Extensions;
    using KnightFrank.Antares.UITests.Pages.Panels;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;
    using Objectivity.Test.Automation.Common.WebElements;

    public class CreateRequirementPage : ProjectPageBase
    {
        private readonly ElementLocator requirementForm = new ElementLocator(Locator.Id, "addRequirementForm");
        // Applicant locators
        private readonly ElementLocator newApplicantButton = new ElementLocator(Locator.CssSelector, "button[ng-click *= 'showContactList']");
        private readonly ElementLocator applicantsList = new ElementLocator(Locator.CssSelector, "div[ng-repeat *= 'requirement.contacts']");
        // Property requirements locators
        private readonly ElementLocator propertyType = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator propertyPriceMin = new ElementLocator(Locator.Id, "price-min");
        private readonly ElementLocator propertyPriceMax = new ElementLocator(Locator.Id, "price-max");
        private readonly ElementLocator propertyBedroomsMin = new ElementLocator(Locator.Id, "bedrooms-min");
        private readonly ElementLocator propertyBedroomsMax = new ElementLocator(Locator.Id, "bedrooms-max");
        private readonly ElementLocator propertyReceptionRoomsMin = new ElementLocator(Locator.Id, "reception-min");
        private readonly ElementLocator propertyReceptionRoomsMax = new ElementLocator(Locator.Id, "reception-max");
        private readonly ElementLocator propertyBathroomsMin = new ElementLocator(Locator.Id, "bathrooms-min");
        private readonly ElementLocator propertyBathroomsMax = new ElementLocator(Locator.Id, "bathrooms-max");
        private readonly ElementLocator propertyParkingSpacesMin = new ElementLocator(Locator.Id, "parking-min");
        private readonly ElementLocator propertyParkingSpacesMax = new ElementLocator(Locator.Id, "parking-max");
        private readonly ElementLocator propertyAreaMin = new ElementLocator(Locator.Id, "area-min");
        private readonly ElementLocator propertyAreaMax = new ElementLocator(Locator.Id, "area-max");
        private readonly ElementLocator propertyLandAreaMin = new ElementLocator(Locator.Id, "land-min");
        private readonly ElementLocator propertyLandAreaMax = new ElementLocator(Locator.Id, "land-max");
        private readonly ElementLocator propertyRequirementsNote = new ElementLocator(Locator.Id, "description");
        // New residential sales requiremen actions
        private readonly ElementLocator saveResidentialSalesRequirement = new ElementLocator(Locator.Id, "saveBtn");
        private readonly ElementLocator panel = new ElementLocator(Locator.CssSelector, ".side-panel.slide-in");

        public CreateRequirementPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public AddressTemplatePage AddressTemplate => new AddressTemplatePage(this.DriverContext);

        public ContactsListPage ContactsList => new ContactsListPage(this.DriverContext);

        public CreateRequirementPage OpenCreateRequirementPage()
        {
            new CommonPage(this.DriverContext).NavigateToPage("create requirement");
            this.Driver.WaitForAngularToFinish();
            return this;
        }

        public CreateRequirementPage SelectPropertyType(string type)
        {
            this.Driver.GetElement<Select>(this.propertyType).SelectByValue(type);
            return this;
        }

        public CreateRequirementPage SetPropertyMinPrice(string minPrice)
        {
            this.Driver.SendKeys(this.propertyPriceMin, minPrice);
            return this;
        }

        public CreateRequirementPage SetPropertyMaxPrice(string maxPrice)
        {
            this.Driver.SendKeys(this.propertyPriceMax, maxPrice);
            return this;
        }

        public CreateRequirementPage SetPropertyBedroomsMin(string bedroomsMin)
        {
            this.Driver.SendKeys(this.propertyBedroomsMin, bedroomsMin);
            return this;
        }

        public CreateRequirementPage SetPropertyBedroomMax(string bedroomsMax)
        {
            this.Driver.SendKeys(this.propertyBedroomsMax, bedroomsMax);
            return this;
        }

        public CreateRequirementPage SetPropertyReceptionRoomsMin(string receptionRoomsMin)
        {
            this.Driver.SendKeys(this.propertyReceptionRoomsMin, receptionRoomsMin);
            return this;
        }

        public CreateRequirementPage SetPropertyReceptionRoomsMax(string receptionRoomsMax)
        {
            this.Driver.SendKeys(this.propertyReceptionRoomsMax, receptionRoomsMax);
            return this;
        }

        public CreateRequirementPage SetPropertyBathroomsMin(string bathroomsMin)
        {
            this.Driver.SendKeys(this.propertyBathroomsMin, bathroomsMin);
            return this;
        }

        public CreateRequirementPage SetPropertyBathroomsMax(string bathroomsMax)
        {
            this.Driver.SendKeys(this.propertyBathroomsMax, bathroomsMax);
            return this;
        }

        public CreateRequirementPage SetPropertyParkingSpacesMin(string parkingSpacesMin)
        {
            this.Driver.SendKeys(this.propertyParkingSpacesMin, parkingSpacesMin);
            return this;
        }

        public CreateRequirementPage SetPropertyParkingSpacesMax(string parkingSpacesMax)
        {
            this.Driver.SendKeys(this.propertyParkingSpacesMax, parkingSpacesMax);
            return this;
        }

        public CreateRequirementPage SetPropertyAreaMin(string areaMin)
        {
            this.Driver.SendKeys(this.propertyAreaMin, areaMin);
            return this;
        }

        public CreateRequirementPage SetPropertyAreaMax(string areaMax)
        {
            this.Driver.SendKeys(this.propertyAreaMax, areaMax);
            return this;
        }

        public CreateRequirementPage SetPropertyLandAreaMin(string landAreaMin)
        {
            this.Driver.SendKeys(this.propertyLandAreaMin, landAreaMin);
            return this;
        }

        public CreateRequirementPage SetPropertyLandAreaMax(string landAreaMax)
        {
            this.Driver.SendKeys(this.propertyLandAreaMax, landAreaMax);
            return this;
        }

        public CreateRequirementPage SetPropertyRequirementsNote(string note)
        {
            this.Driver.SendKeys(this.propertyRequirementsNote, note);
            return this;
        }

        public CreateRequirementPage SaveRequirement()
        {
            this.Driver.GetElement(this.saveResidentialSalesRequirement).Click();
            this.Driver.WaitForAngularToFinish(BaseConfiguration.MediumTimeout);
            return this;
        }

        public CreateRequirementPage AddApplicants()
        {
            this.Driver.GetElement(this.newApplicantButton).Click();
            return this;
        }

        public List<string> GetApplicants()
        {
            return this.Driver.GetElements(this.applicantsList).Select(el => el.Text).ToList();
        }

        public bool IsRequirementFormPresent()
        {
            return this.Driver.IsElementPresent(this.requirementForm, BaseConfiguration.MediumTimeout);
        }

        public CreateRequirementPage WaitForSidePanelToShow()
        {
            this.Driver.WaitForElementToBeDisplayed(this.panel, BaseConfiguration.MediumTimeout);
            return this;
        }

        public CreateRequirementPage WaitForSidePanelToHide()
        {
            this.Driver.WaitUntilElementIsNoLongerFound(this.panel, BaseConfiguration.MediumTimeout);
            return this;
        }
    }
}
