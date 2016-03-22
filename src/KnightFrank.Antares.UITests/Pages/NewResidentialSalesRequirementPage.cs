namespace KnightFrank.Antares.UITests.Pages
{
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;
    using Objectivity.Test.Automation.Common.WebElements;

    public class NewResidentialSalesRequirementPage : ProjectPageBase
    {
        // Applicant locators
        private readonly ElementLocator newApplicantButton = new ElementLocator(Locator.CssSelector, "button[ng-click = 'vm.showContactList()']");
        private readonly ElementLocator applicantsList = new ElementLocator(Locator.CssSelector, "div[ng-repeat = 'c in vm.requirement.contacts']");
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

        public AddressTemplatePage AddressTemplate => new AddressTemplatePage(this.DriverContext);

        public ContactsListPage ContactsList => new ContactsListPage(this.DriverContext);

        public NewResidentialSalesRequirementPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public NewResidentialSalesRequirementPage OpenNewResidentialSalesRequirementPage()
        {
            new CommonPage(this.DriverContext).NavigateToPage("New Residential Sales Requirement");
            this.Driver.WaitForAngularToFinish();
            return this;
        }

        public NewResidentialSalesRequirementPage SelectPropertyType(string type)
        {
            this.Driver.GetElement<Select>(this.propertyType).SelectByValue(type);
            return this;
        }

        public NewResidentialSalesRequirementPage SetPropertyMinPrice(string minPrice)
        {
            this.Driver.SendKeys(this.propertyPriceMin, minPrice);
            return this;
        }

        public NewResidentialSalesRequirementPage SetPropertyMaxPrice(string maxPrice)
        {
            this.Driver.SendKeys(this.propertyPriceMax, maxPrice);
            return this;
        }

        public NewResidentialSalesRequirementPage SetPropertyBedroomsMin(string bedroomsMin)
        {
            this.Driver.SendKeys(this.propertyBedroomsMin, bedroomsMin);
            return this;
        }

        public NewResidentialSalesRequirementPage SetPropertyBedroomMax(string bedroomsMax)
        {
            this.Driver.SendKeys(this.propertyBedroomsMax, bedroomsMax);
            return this;
        }

        public NewResidentialSalesRequirementPage SetPropertyReceptionRoomsMin(string receptionRoomsMin)
        {
            this.Driver.SendKeys(this.propertyReceptionRoomsMin, receptionRoomsMin);
            return this;
        }

        public NewResidentialSalesRequirementPage SetPropertyReceptionRoomsMax(string receptionRoomsMax)
        {
            this.Driver.SendKeys(this.propertyReceptionRoomsMax, receptionRoomsMax);
            return this;
        }

        public NewResidentialSalesRequirementPage SetPropertyBathroomsMin(string bathroomsMin)
        {
            this.Driver.SendKeys(this.propertyBathroomsMin, bathroomsMin);
            return this;
        }

        public NewResidentialSalesRequirementPage SetPropertyBathroomsMax(string bathroomsMax)
        {
            this.Driver.SendKeys(this.propertyBathroomsMax, bathroomsMax);
            return this;
        }

        public NewResidentialSalesRequirementPage SetPropertyParkingSpacesMin(string parkingSpacesMin)
        {
            this.Driver.SendKeys(this.propertyParkingSpacesMin, parkingSpacesMin);
            return this;
        }

        public NewResidentialSalesRequirementPage SetPropertyParkingSpacesMax(string parkingSpacesMax)
        {
            this.Driver.SendKeys(this.propertyParkingSpacesMax, parkingSpacesMax);
            return this;
        }

        public NewResidentialSalesRequirementPage SetPropertyAreaMin(string areaMin)
        {
            this.Driver.SendKeys(this.propertyAreaMin, areaMin);
            return this;
        }

        public NewResidentialSalesRequirementPage SetPropertyAreaMax(string areaMax)
        {
            this.Driver.SendKeys(this.propertyAreaMax, areaMax);
            return this;
        }

        public NewResidentialSalesRequirementPage SetPropertyLandAreaMin(string landAreaMin)
        {
            this.Driver.SendKeys(this.propertyLandAreaMin, landAreaMin);
            return this;
        }

        public NewResidentialSalesRequirementPage SetPropertyLandAreaMax(string landAreaMax)
        {
            this.Driver.SendKeys(this.propertyLandAreaMax, landAreaMax);
            return this;
        }

        public NewResidentialSalesRequirementPage SetPropertyRequirementsNote(string note)
        {
            this.Driver.SendKeys(this.propertyRequirementsNote, note);
            return this;
        }

        public void SaveNewResidentialSalesRequirement()
        {
            this.Driver.GetElement(this.saveResidentialSalesRequirement).Click();
            this.Driver.WaitForAngularToFinish(BaseConfiguration.MediumTimeout);
        }

        public void AddNewApllicantForResidentialSalesRequirement()
        {
            this.Driver.GetElement(this.newApplicantButton).Click();
        }

        public List<string> GetApplicants()
        {
            return this.Driver.GetElements(this.applicantsList).Select(el => el.Text).ToList();
        } 
    }
}
