namespace KnightFrank.Antares.UITests.Pages
{
    using System.Linq;

    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;
    using Objectivity.Test.Automation.Common.WebElements;

    using OpenQA.Selenium;

    public class CreatePropertyPage : ProjectPageBase
    {
        private readonly ElementLocator propertyType = new ElementLocator(Locator.Id, "type");
        private readonly ElementLocator propertyTypeLink = new ElementLocator(Locator.CssSelector, "a[ng-click *= 'changeDivision']:not([class *= 'ng-hide'])");
        private readonly ElementLocator saveButton = new ElementLocator(Locator.Id, "saveBtn");
        // Property details
        private readonly ElementLocator minBedrooms = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator maxBedrooms = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator minReceptionRooms = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator maxReceptionRooms = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator minBathrooms = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator maxBathrooms = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator minParkingSpaces = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator maxParkingSpaces = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator minPropertyArea = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator maxPropertyArea = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator minLandArea = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator maxLandArea = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator minGuestRooms = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator maxGuestRooms = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator minFunctionRooms = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator maxFunctionRooms = new ElementLocator(Locator.Id, string.Empty);

        public CreatePropertyPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public AddressTemplatePage AddressTemplate => new AddressTemplatePage(this.DriverContext);

        public CreatePropertyPage OpenCreatePropertyPage()
        {
            new CommonPage(this.DriverContext).NavigateToPage("create property");
            return this;
        }

        public CreatePropertyPage SelectPropertyType(string type)
        {
            var select = this.Driver.GetElement<Select>(this.propertyType);
            IWebElement element = select.SelectElement().Options.Single(o => o.Text.Trim().Equals(type));
            select.SelectByIndex(select.SelectElement().Options.IndexOf(element));
            return this;
        }

        public CreatePropertyPage SelectType(string type)
        {
            switch (type.ToLower())
            {
                case "residential":
                    if (!this.Driver.GetElement(this.propertyTypeLink).Text.ToLower().Contains("commercial"))
                    {
                        this.Driver.GetElement(this.propertyTypeLink).Click();
                    }
                    break;
                case "commercial":
                    if (!this.Driver.GetElement(this.propertyTypeLink).Text.ToLower().Contains("residential"))
                    {
                        this.Driver.GetElement(this.propertyTypeLink).Click();
                    }
                    break;
            }
            this.Driver.WaitForAngularToFinish();
            return this;
        }

        public ViewPropertyPage SaveProperty()
        {
            this.Driver.GetElement(this.saveButton).Click();
            this.Driver.WaitForAngularToFinish();
            return new ViewPropertyPage(this.DriverContext);
        }

        public CreatePropertyPage SetMinBedrooms(string min)
        {
            this.Driver.SendKeys(this.minBedrooms, min);
            return this;
        }

        public CreatePropertyPage SetMaxBedrooms(string max)
        {
            this.Driver.SendKeys(this.maxBedrooms, max);
            return this;
        }

        public CreatePropertyPage SetMinReceptionRooms(string min)
        {
            this.Driver.SendKeys(this.minReceptionRooms, min);
            return this;
        }

        public CreatePropertyPage SetMaxReceptionRooms(string max)
        {
            this.Driver.SendKeys(this.maxReceptionRooms, max);
            return this;
        }

        public CreatePropertyPage SetMinBathrooms(string min)
        {
            this.Driver.SendKeys(this.minBathrooms, min);
            return this;
        }

        public CreatePropertyPage SetMaxBathrooms(string max)
        {
            this.Driver.SendKeys(this.maxBathrooms, max);
            return this;
        }

        public CreatePropertyPage SetMinParkingSpaces(string min)
        {
            this.Driver.SendKeys(this.minParkingSpaces, min);
            return this;
        }

        public CreatePropertyPage SetMaxParkingSpaces(string max)
        {
            this.Driver.SendKeys(this.maxParkingSpaces, max);
            return this;
        }

        public CreatePropertyPage SetMinPropertyArea(string min)
        {
            this.Driver.SendKeys(this.minPropertyArea, min);
            return this;
        }

        public CreatePropertyPage SetMaxPropertyArea(string max)
        {
            this.Driver.SendKeys(this.maxPropertyArea, max);
            return this;
        }

        public CreatePropertyPage SetMinLandArea(string min)
        {
            this.Driver.SendKeys(this.minLandArea, min);
            return this;
        }

        public CreatePropertyPage SetMaxLandArea(string max)
        {
            this.Driver.SendKeys(this.maxLandArea, max);
            return this;
        }

        public CreatePropertyPage SetMinGuestRooms(string min)
        {
            this.Driver.SendKeys(this.minGuestRooms, min);
            return this;
        }

        public CreatePropertyPage SetMaxGuestRooms(string max)
        {
            this.Driver.SendKeys(this.maxGuestRooms, max);
            return this;
        }

        public CreatePropertyPage SetMinFunctionRooms(string min)
        {
            this.Driver.SendKeys(this.minFunctionRooms, min);
            return this;
        }

        public CreatePropertyPage SetMaxFunctionRooms(string max)
        {
            this.Driver.SendKeys(this.maxFunctionRooms, max);
            return this;
        }
    }

    internal class PropertyDetails
    {
        public string MinBedrooms { get; set; }
        public string MaxBedrooms { get; set; }
        public string MinReceptionRooms { get; set; }
        public string MaxReceptionRooms { get; set; }
        public string MinBathrooms { get; set; }
        public string MaxBathrooms { get; set; }
        public string MinParkingSpaces { get; set; }
        public string MaxParkingSpaces { get; set; }
        public string MinPropertyArea { get; set; }
        public string MaxPropertyArea { get; set; }
        public string MinLandArea { get; set; }
        public string MaxLandArea { get; set; }
        public string MinGuestRooms { get; set; }
        public string MaxGuestRooms { get; set; }
        public string MinFunctionRooms { get; set; }
        public string MaxFunctionRooms { get; set; }
    }
}
