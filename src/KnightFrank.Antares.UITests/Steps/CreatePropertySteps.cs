namespace KnightFrank.Antares.UITests.Steps
{
    using System;

    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.UITests.Pages;

    using Objectivity.Test.Automation.Common;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

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
        [When(@"User navigates to create property page")]
        public void OpenCreatePropertyPage()
        {
            CreatePropertyPage page = new CreatePropertyPage(this.driverContext).OpenAddPropertyPage();
            this.scenarioContext["CreatePropertyPage"] = page;
        }

        [When(@"User selects (.*) country on create property page")]
        public void SelectCountryFromDropDownList(string country)
        {
            this.scenarioContext.Get<CreatePropertyPage>("CreatePropertyPage").AddressTemplate.SelectPropertyCountry(country);
        }

        [When(@"User fills in address details on create property page")]
        public void FillInAddressDetails(Table table)
        {
            var address = table.CreateInstance<Address>();
            var page = this.scenarioContext.Get<CreatePropertyPage>("CreatePropertyPage");

            page.AddressTemplate
                .SetPropertyNumber(address.PropertyNumber)
                .SetPropertyName(address.PropertyName)
                .SetPropertyAddressLine2(address.Line2)
                .SetPropertyAddressLine3(address.Line3)
                .SetPropertyPostCode(address.Postcode)
                .SetPropertyCity(address.City)
                .SetPropertyCounty(address.County);
        }

        [When(@"User selects (.*) property and (.*) type on create property page")]
        public void SelectPropertyTypes(string type, string propertyType)
        {
            this.scenarioContext.Get<CreatePropertyPage>("CreatePropertyPage").SelectType(type).SelectPropertyType(propertyType);
        }

        [When(@"User clicks save button on create property page")]
        public void ClickSaveButton()
        {
            var page = this.scenarioContext.Get<CreatePropertyPage>("CreatePropertyPage");
            this.scenarioContext.Set(page.SaveProperty(), "ViewPropertyPage");
        }
    }
}
