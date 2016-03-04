namespace KnightFrank.Antares.UITests.Pages
{
    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;
    using Objectivity.Test.Automation.Common.WebElements;

    public class NewResidentialSalesRequirementPage : ProjectPageBase
    {
        // Applicant locators
        private readonly ElementLocator newApplicantButton = new ElementLocator(Locator.Id, string.Empty);
        // Property requirements locators
        private readonly ElementLocator propertyType = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator propertyPriceMin = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator propertyPriceMax = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator propertyBedroomsMin = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator propertyBedroomsMax = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator propertyReceptionRoomsMin = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator propertyReceptionRoomsMax = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator propertyBathroomsMin = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator propertyBathroomsMax = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator propertyParkingSpacesMin = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator propertyParkingSpacesMax = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator propertyAreaMin = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator propertyAreaMax = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator propertyLandAreaMin = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator propertyLandAreaMax = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator propertyRequirementsNote = new ElementLocator(Locator.Id, string.Empty);
        // New residential sales requiremen actions
        private readonly ElementLocator saveResidentialSalesRequirement = new ElementLocator(Locator.Id, string.Empty);

        private AddressTemplatePage AddressTemplate => new AddressTemplatePage(this.DriverContext);

        public NewResidentialSalesRequirementPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public void AddNewApllicantForResidentialSalesRequirement()
        {
            this.Driver.GetElement(this.newApplicantButton).Click();
        }

        public NewResidentialSalesRequirementPage OpenNewResidentialSalesRequirementPage()
        {
            new CommonPage(this.DriverContext).NavigateToPage("New Residential Sales Requirement");
            return this;
        }

        public NewResidentialSalesRequirementPage SelectLocationCountry(string country)
        {
            this.AddressTemplate.SelectPropertyCountry(country);
            return this;
        }

        public NewResidentialSalesRequirementPage SetLocationStreetName(string streetName)
        {
            this.AddressTemplate.SetPropertyAddressLine2(streetName);
            return this;
        }

        public NewResidentialSalesRequirementPage SetLocationPostCode(string postCode)
        {
            this.AddressTemplate.SetPropertyPostCode(postCode);
            return this;
        }

        public NewResidentialSalesRequirementPage SetLocationCity(string city)
        {
            this.AddressTemplate.SetPropertyCity(city);
            return this;
        }

        public NewResidentialSalesRequirementPage SelectPropertyType(string type)
        {
            this.Driver.GetElement<Select>(this.propertyType).SelectByValue(type);
            return this;
        }

        public NewResidentialSalesRequirementPage SetPropertyMinPrice(string minPrice)
        {
            this.Driver.GetElement(this.propertyPriceMin).SendKeys(minPrice);
            return this;
        }

        public NewResidentialSalesRequirementPage SetPropertyMaxPrice(string maxPrice)
        {
            this.Driver.GetElement(this.propertyPriceMax).SendKeys(maxPrice);
            return this;
        }

        public NewResidentialSalesRequirementPage SetPropertyBedroomsMin(string bedroomsMin)
        {
            this.Driver.GetElement(this.propertyBedroomsMin).SendKeys(bedroomsMin);
            return this;
        }

        public NewResidentialSalesRequirementPage SetPropertyBedroomMax(string bedroomsMax)
        {
            this.Driver.GetElement(this.propertyBedroomsMax).SendKeys(bedroomsMax);
            return this;
        }

        public NewResidentialSalesRequirementPage SetPropertyReceptionRoomsMin(string receptionRoomsMin)
        {
            this.Driver.GetElement(this.propertyReceptionRoomsMin).SendKeys(receptionRoomsMin);
            return this;
        }

        public NewResidentialSalesRequirementPage SetPropertyReceptionRoomsMax(string receptionRoomsMax)
        {
            this.Driver.GetElement(this.propertyReceptionRoomsMax).SendKeys(receptionRoomsMax);
            return this;
        }

        public NewResidentialSalesRequirementPage SetPropertyBathroomsMin(string bathroomsMin)
        {
            this.Driver.GetElement(this.propertyBathroomsMin).SendKeys(bathroomsMin);
            return this;
        }

        public NewResidentialSalesRequirementPage SetPropertyBathroomsMax(string bathroomsMax)
        {
            this.Driver.GetElement(this.propertyBathroomsMax).SendKeys(bathroomsMax);
            return this;
        }

        public NewResidentialSalesRequirementPage SetPropertyParkingSpacesMin(string parkingSpacesMin)
        {
            this.Driver.GetElement(this.propertyParkingSpacesMin).SendKeys(parkingSpacesMin);
            return this;
        }

        public NewResidentialSalesRequirementPage SetPropertyParkingSpacesMax(string parkingSpacesMax)
        {
            this.Driver.GetElement(this.propertyParkingSpacesMax).SendKeys(parkingSpacesMax);
            return this;
        }

        public NewResidentialSalesRequirementPage SetPropertyAreaMin(string areaMin)
        {
            this.Driver.GetElement(this.propertyAreaMin).SendKeys(areaMin);
            return this;
        }

        public NewResidentialSalesRequirementPage SetPropertyAreaMax(string areaMax)
        {
            this.Driver.GetElement(this.propertyAreaMax).SendKeys(areaMax);
            return this;
        }

        public NewResidentialSalesRequirementPage SetPropertyLandAreaMin(string landAreaMin)
        {
            this.Driver.GetElement(this.propertyLandAreaMin).SendKeys(landAreaMin);
            return this;
        }

        public NewResidentialSalesRequirementPage SetPropertyLandAreaMax(string landAreaMax)
        {
            this.Driver.GetElement(this.propertyLandAreaMax).SendKeys(landAreaMax);
            return this;
        }

        public NewResidentialSalesRequirementPage SetPropertyRequirementsNote(string note)
        {
            this.Driver.GetElement(this.propertyRequirementsNote).SendKeys(note);
            return this;
        }

        public void SaveNewResidentialSalesRequirement()
        {
            this.Driver.GetElement(this.saveResidentialSalesRequirement).Click();
        }
    }
}
