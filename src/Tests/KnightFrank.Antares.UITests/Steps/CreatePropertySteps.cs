namespace KnightFrank.Antares.UITests.Steps
{
    using System;

    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.Dal.Model.Attribute;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.UITests.Pages;

    using Objectivity.Test.Automation.Common;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    using Xunit;

    [Binding]
    public class CreatePropertySteps
    {
        private readonly DriverContext driverContext;
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly ScenarioContext scenarioContext;
        private CreatePropertyPage page;

        public CreatePropertySteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }

            this.scenarioContext = scenarioContext;
            this.driverContext = this.scenarioContext["DriverContext"] as DriverContext;

            if (this.page == null)
            {
                this.page = new CreatePropertyPage(this.driverContext);
            }
        }

        [Given(@"User navigates to create property page")]
        [When(@"User navigates to create property page")]
        public void OpenCreatePropertyPage()
        {
            this.page = new CreatePropertyPage(this.driverContext).OpenCreatePropertyPage();
        }
        
        [When(@"User navigates to edit property page with id")]
        public void OpenEditPropertyPage()
        {
            Guid propertyId = this.scenarioContext.Get<Property>("Property").Id;
            this.page = new CreatePropertyPage(this.driverContext).OpenEditPropertyPage(propertyId.ToString());
        }

        [When(@"User selects (.*) country on create property page")]
        public void SelectCountryFromDropDownList(string country)
        {
            this.page.AddressTemplate.SelectPropertyCountry(country);
        }

        [When(@"User fills in address details on create property page")]
        [When(@"User fills in address details on edit property page")]
        public void FillInAddressDetails(Table table)
        {
            var address = table.CreateInstance<Address>();

            this.page
                .AddressTemplate
                .SetPropertyNumber(address.PropertyNumber)
                .SetPropertyName(address.PropertyName)
                .SetPropertyAddressLine1(address.Line1)
                .SetPropertyAddressLine2(address.Line2)
                .SetPropertyAddressLine3(address.Line3)
                .SetPropertyPostCode(address.Postcode)
                .SetPropertyCity(address.City)
                .SetPropertyCounty(address.County);
        }

        [When(@"User fills in property details on create property page")]
        [When(@"User fills in property details on edit property page")]
        public void FillInPropertyDetails(Table table)
        {
            var details = table.CreateInstance<AttributeValues>();

            this.page
                .SetMinBedrooms(details.MinBedrooms)
                .SetMaxBedrooms(details.MaxBedrooms)
                .SetMinReceptionRooms(details.MinReceptions)
                .SetMaxReceptionRooms(details.MaxReceptions)
                .SetMinBathrooms(details.MinBathrooms)
                .SetMaxBathrooms(details.MaxBathrooms)
                .SetMinParkingSpaces(details.MinCarParkingSpaces)
                .SetMaxParkingSpaces(details.MaxCarParkingSpaces)
                .SetMinPropertyArea(details.MinArea)
                .SetMaxPropertyArea(details.MaxArea)
                .SetMinLandArea(details.MinLandArea)
                .SetMaxLandArea(details.MaxLandArea)
                .SetMinGuestRooms(details.MinGuestRooms)
                .SetMaxGuestRooms(details.MaxGuestRooms)
                .SetMinFunctionRooms(details.MinFunctionRooms)
                .SetMaxFunctionRooms(details.MaxFunctionRooms);
        }

        [When(@"User selects (.*) property and (.*) type on create property page")]
        [When(@"User selects (.*) property and (.*) type on edit property page")]
        public void SelectPropertyTypes(string type, string propertyType)
        {
            this.page.SelectType(type).SelectPropertyType(propertyType);
        }

        [When(@"User clicks save property button on create property page")]
        [When(@"User clicks save property button on edit property page")]
        public void ClickSaveButton()
        {
            this.page.SaveProperty();
        }

        [When(@"User selects property characteristics on edit property page")]
        [When(@"User selects property characteristics on create property page")]
        public void SelectCharacteristics(Table table)
        {
            foreach (Characteristic characteristic in table.CreateSet<Characteristic>())
            {
                this.page.SelectCharacteristic(characteristic.Name);
                if (characteristic.Comment != null)
                {
                    this.page.AddCommentToCharacteristic(characteristic.Name, characteristic.Comment);
                }
            }
        }

        [Then(@"Property form on create property page should be displayed")]
        public void CheckIfPropertyTypeIsDisplayed()
        {
            Assert.True(new CreatePropertyPage(this.driverContext).IsPropertyFormPresent());
        }
    }
}
