namespace KnightFrank.Antares.UITests.Steps
{
    using System;
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.UITests.Pages;

    using Objectivity.Test.Automation.Common;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    using Xunit;

    [Binding]
    public class CreatePropertySteps
    {
        private readonly DriverContext driverContext;
        private readonly ScenarioContext scenarioContext;

        public CreatePropertySteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }

            this.scenarioContext = scenarioContext;
            this.driverContext = this.scenarioContext["DriverContext"] as DriverContext;
        }

        [Given(@"User navigates to create property page")]
        public void OpenCreatePropertyPage()
        {
            var createPropertyPage = new CreatePropertyPage(this.driverContext);
            this.scenarioContext["CreatePropertyPage"] = createPropertyPage;
        }

        [When(@"User selects country on create property page")]
        public void SelectCountryFromDropDownList(Table table)
        {
            var tableCountry = table.CreateInstance<Address>();
            this.scenarioContext.Get<CreatePropertyPage>("CreatePropertyPage")
                .AddressTemplate.SelectPropertyCountry(tableCountry.Country.IsoCode);
        }

        [When(@"User fills in address details on create property page")]
        public void FillInAddressDetails(Table table)
        {
            var addressDetails = table.CreateInstance<Address>();
            var createPropertyPage = this.scenarioContext.Get<CreatePropertyPage>("CreatePropertyPage");

            createPropertyPage.AddressTemplate
                              .SetPropertyNumber(addressDetails.PropertyNumber)
                              .SetPropertyName(addressDetails.PropertyName)
                              .SetPropertyAddressLine2(addressDetails.Line2)
                              .SetPropertyAddressLine3(addressDetails.Line3)
                              .SetPropertyPostCode(addressDetails.Postcode)
                              .SetPropertyCity(addressDetails.City)
                              .SetPropertyCounty(addressDetails.County);
        }

        [When(@"User selects property types on create property page")]
        public void SelectPropertyTypes(Table table)
        {
            var types = table.CreateInstance<List<PropertyType>>();
            var createPropertyPage = this.scenarioContext.Get<CreatePropertyPage>("CreatePropertyPage");

            foreach (PropertyType type in types)
            {
                createPropertyPage.SelectPropertyType(type.Type);
            }
        }

        [When(@"User clicks save button on create property page")]
        public void ClickSaveButton()
        {
            this.scenarioContext.Get<CreatePropertyPage>("CreatePropertyPage").SaveProperty();
        }

        [Then(@"New property should be created with address details")]
        public void CheckIfPropertyCreated(Table table)
        {
            var address = table.CreateInstance<Address>();
            var page = new ViewPropertyPage(this.driverContext);
           
            Assert.Equal(address.Country.IsoCode, page.GetCountry());
            Assert.Equal(address.County, page.GetCounty());
            Assert.Equal(address.PropertyNumber, page.GetPropertyNumber());
            Assert.Equal(address.PropertyName, page.GetPropertyName());
            Assert.Equal(address.Line2, page.GetAddressLine2());
            Assert.Equal(address.Postcode, page.GetPostCode());
            Assert.Equal(address.City, page.GetCity());
        }
    }

    internal class PropertyType
    {
        public string Type { get; set; }
    }
}
