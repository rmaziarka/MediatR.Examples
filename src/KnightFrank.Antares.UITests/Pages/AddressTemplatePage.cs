namespace KnightFrank.Antares.UITests.Pages
{
    using System.Collections.Generic;

    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;
    using Objectivity.Test.Automation.Common.WebElements;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;

    public class AddressTemplatePage : ProjectPageBase
    {
        private readonly ElementLocator propertyNumber = new ElementLocator(Locator.Id, "propertyNumber");
        private readonly ElementLocator propertyName = new ElementLocator(Locator.Id, "propertyName");
        private readonly ElementLocator propertyAddressLine1 = new ElementLocator(Locator.Id, "line1");
        private readonly ElementLocator propertyAddressLine2 = new ElementLocator(Locator.Id, "line2");
        private readonly ElementLocator propertyAddressLine3 = new ElementLocator(Locator.Id, "line3");
        private readonly ElementLocator propertyCity = new ElementLocator(Locator.Id, "city");
        private readonly ElementLocator propertyCounty = new ElementLocator(Locator.Id, "county");
        private readonly ElementLocator propertyPostcode = new ElementLocator(Locator.Id, "postcode");
        private readonly ElementLocator propertyCountry = new ElementLocator(Locator.Id, "country");

        public AddressTemplatePage(DriverContext driverContext) : base(driverContext)
        {
        }

        // Property/Unit/Flat Number
        public AddressTemplatePage SetPropertyNumber(string number)
        {
            this.Driver.SendKeys(this.propertyNumber, number);
            return this;
        }

        // Property/Building Name
        public AddressTemplatePage SetPropertyName(string name)
        {
            this.Driver.SendKeys(this.propertyName, name);
            return this;
        }

        // Address Line 1/Street Address/Street Number
        public AddressTemplatePage SetPropertyAddressLine1(string address)
        {
            this.Driver.SendKeys(this.propertyAddressLine1, address);
            return this;
        }

        // Address Line 2/Street Name
        public AddressTemplatePage SetPropertyAddressLine2(string address)
        {
            this.Driver.SendKeys(this.propertyAddressLine2, address);
            return this;
        }

        // Address Line 3
        public AddressTemplatePage SetPropertyAddressLine3(string address)
        {
            this.Driver.SendKeys(this.propertyAddressLine3, address);
            return this;
        }

        // City
        public AddressTemplatePage SetPropertyCity(string city)
        {
            this.Driver.SendKeys(this.propertyCity, city);
            return this;
        }

        // County/State/Province
        public AddressTemplatePage SetPropertyCounty(string county)
        {
            this.Driver.SendKeys(this.propertyCounty, county);
            return this;
        }

        // Postcode/Pincode/Postalcode
        public AddressTemplatePage SetPropertyPostCode(string postcode)
        {
            this.Driver.SendKeys(this.propertyPostcode, postcode);
            return this;
        }

        // Country
        public AddressTemplatePage SelectPropertyCountry(string country)
        {
            this.Driver.WaitForAngularToFinish();
            var select = this.Driver.GetElement<Select>(this.propertyCountry);
            if (!select.SelectElement().SelectedOption.Text.Equals(country))
            {
                select.SelectByText(country);
            }
            this.Driver.WaitForAngularToFinish();
            return this;
        }
    }
}
