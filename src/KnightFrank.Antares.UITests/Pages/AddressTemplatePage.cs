namespace KnightFrank.Antares.UITests.Pages
{
    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;
    using Objectivity.Test.Automation.Common.WebElements;

    public class AddressTemplatePage : ProjectPageBase
    {
        private readonly ElementLocator propertyNumber = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator propertyName = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator propertyAddressLine1 = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator propertyAddressLine2 = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator propertyAddressLine3 = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator propertyCity = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator propertyCounty = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator propertyPostcode = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator propertyCountry = new ElementLocator(Locator.Id, string.Empty);

        public AddressTemplatePage(DriverContext driverContext) : base(driverContext)
        {
        }

        // Property/Unit/Flat Number
        public AddressTemplatePage SetPropertyNumber(string number)
        {
            this.Driver.GetElement(this.propertyNumber).SendKeys(number);
            return this;
        }

        // Property/Building Name
        public AddressTemplatePage SetPropertyName(string name)
        {
            this.Driver.GetElement(this.propertyName).SendKeys(name);
            return this;
        }

        // Address Line 1/Street Address/Street Number
        public AddressTemplatePage SetPropertyAddressLine1(string address)
        {
            this.Driver.GetElement(this.propertyAddressLine1).SendKeys(address);
            return this;
        }

        // Address Line 2/Street Name
        public AddressTemplatePage SetPropertyAddressLine2(string address)
        {
            this.Driver.GetElement(this.propertyAddressLine2).SendKeys(address);
            return this;
        }

        // Address Line 3
        public AddressTemplatePage SetPropertyAddressLine3(string address)
        {
            this.Driver.GetElement(this.propertyAddressLine3).SendKeys(address);
            return this;
        }

        // City
        public AddressTemplatePage SetPropertyCity(string city)
        {
            this.Driver.GetElement(this.propertyCity).SendKeys(city);
            return this;
        }

        // County/State/Province
        public AddressTemplatePage SetPropertyCounty(string county)
        {
            this.Driver.GetElement(this.propertyCounty).SendKeys(county);
            return this;
        }

        // Postcode/Pincode/Postalcode
        public AddressTemplatePage SetPropertyPostCode(string postcode)
        {
            this.Driver.GetElement(this.propertyPostcode).SendKeys(postcode);
            return this;
        }

        // Country
        public AddressTemplatePage SelectPropertyCountry(string country)
        {
            this.Driver.GetElement<Select>(this.propertyCountry).SelectByValue(country);
            return this;
        }
    }
}
