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
        private readonly ElementLocator propertyForm = new ElementLocator(Locator.CssSelector, "property-add");
        private readonly ElementLocator propertyType = new ElementLocator(Locator.Id, "type");
        private readonly ElementLocator propertyTypeLink = new ElementLocator(Locator.CssSelector, "a[ng-click *= 'changeDivision']:not([class *= 'ng-hide'])");
        private readonly ElementLocator saveButton = new ElementLocator(Locator.Id, "saveBtn");
        // Property details
        private readonly ElementLocator minBedrooms = new ElementLocator(Locator.Id, "minBedrooms");
        private readonly ElementLocator maxBedrooms = new ElementLocator(Locator.Id, "maxBedrooms");
        private readonly ElementLocator minReceptionRooms = new ElementLocator(Locator.Id, "minReceptions");
        private readonly ElementLocator maxReceptionRooms = new ElementLocator(Locator.Id, "maxReceptions");
        private readonly ElementLocator minBathrooms = new ElementLocator(Locator.Id, "minBathrooms");
        private readonly ElementLocator maxBathrooms = new ElementLocator(Locator.Id, "maxBathrooms");
        private readonly ElementLocator minParkingSpaces = new ElementLocator(Locator.Id, "minCarParkingSpaces");
        private readonly ElementLocator maxParkingSpaces = new ElementLocator(Locator.Id, "maxCarParkingSpaces");
        private readonly ElementLocator minPropertyArea = new ElementLocator(Locator.Id, "minArea");
        private readonly ElementLocator maxPropertyArea = new ElementLocator(Locator.Id, "maxArea");
        private readonly ElementLocator minLandArea = new ElementLocator(Locator.Id, "minLandArea");
        private readonly ElementLocator maxLandArea = new ElementLocator(Locator.Id, "maxLandArea");
        private readonly ElementLocator minGuestRooms = new ElementLocator(Locator.Id, "minGuestRooms");
        private readonly ElementLocator maxGuestRooms = new ElementLocator(Locator.Id, "maxGuestRooms");
        private readonly ElementLocator minFunctionRooms = new ElementLocator(Locator.Id, "minFunctionRooms");
        private readonly ElementLocator maxFunctionRooms = new ElementLocator(Locator.Id, "maxFunctionRooms");
        // Property characteristics
        private readonly ElementLocator characteristic = new ElementLocator(Locator.XPath, "//label[contains(text(),'{0}')]/input");
        private readonly ElementLocator characteristicCommentIcon = new ElementLocator(Locator.XPath, "//label[contains(text(),'{0}')]/following-sibling::button");
        private readonly ElementLocator characteristicComment = new ElementLocator(Locator.XPath, "//label[contains(text(),'{0}')]/following-sibling::input");

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
            this.Driver.WaitForAngularToFinish();
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

        public CreatePropertyPage SetMinBedrooms(int? min)
        {
            this.Driver.SendKeys(this.minBedrooms, min);
            return this;
        }

        public CreatePropertyPage SetMaxBedrooms(int? max)
        {
            this.Driver.SendKeys(this.maxBedrooms, max);
            return this;
        }

        public CreatePropertyPage SetMinReceptionRooms(int? min)
        {
            this.Driver.SendKeys(this.minReceptionRooms, min);
            return this;
        }

        public CreatePropertyPage SetMaxReceptionRooms(int? max)
        {
            this.Driver.SendKeys(this.maxReceptionRooms, max);
            return this;
        }

        public CreatePropertyPage SetMinBathrooms(int? min)
        {
            this.Driver.SendKeys(this.minBathrooms, min);
            return this;
        }

        public CreatePropertyPage SetMaxBathrooms(int? max)
        {
            this.Driver.SendKeys(this.maxBathrooms, max);
            return this;
        }

        public CreatePropertyPage SetMinParkingSpaces(int? min)
        {
            this.Driver.SendKeys(this.minParkingSpaces, min);
            return this;
        }

        public CreatePropertyPage SetMaxParkingSpaces(int? max)
        {
            this.Driver.SendKeys(this.maxParkingSpaces, max);
            return this;
        }

        public CreatePropertyPage SetMinPropertyArea(double? min)
        {
            this.Driver.SendKeys(this.minPropertyArea, min);
            return this;
        }

        public CreatePropertyPage SetMaxPropertyArea(double? max)
        {
            this.Driver.SendKeys(this.maxPropertyArea, max);
            return this;
        }

        public CreatePropertyPage SetMinLandArea(double? min)
        {
            this.Driver.SendKeys(this.minLandArea, min);
            return this;
        }

        public CreatePropertyPage SetMaxLandArea(double? max)
        {
            this.Driver.SendKeys(this.maxLandArea, max);
            return this;
        }

        public CreatePropertyPage SetMinGuestRooms(int? min)
        {
            this.Driver.SendKeys(this.minGuestRooms, min);
            return this;
        }

        public CreatePropertyPage SetMaxGuestRooms(int? max)
        {
            this.Driver.SendKeys(this.maxGuestRooms, max);
            return this;
        }

        public CreatePropertyPage SetMinFunctionRooms(int? min)
        {
            this.Driver.SendKeys(this.minFunctionRooms, min);
            return this;
        }

        public CreatePropertyPage SetMaxFunctionRooms(int? max)
        {
            this.Driver.SendKeys(this.maxFunctionRooms, max);
            return this;
        }

        public CreatePropertyPage SelectCharacteristic(string value)
        {
            this.Driver.ScrollIntoMiddle(this.characteristic.Format(value));
            this.Driver.GetElement<Checkbox>(this.characteristic.Format(value)).TickCheckbox();
            return this;
        }

        public CreatePropertyPage AddCommentToCharacteristic(string name, string comment)
        {
            this.Driver.GetElement(this.characteristicCommentIcon.Format(name)).Click();
            this.Driver.SendKeys(this.characteristicComment.Format(name), comment);
            return this;
        }

        public bool IsPropertyFormPresent()
        {
            return this.Driver.IsElementPresent(this.propertyForm, BaseConfiguration.LongTimeout);
        }
    }

    internal class Characteristic
    {
        public string Name { get; set; }

        public string Comment { get; set; }
    }
}
