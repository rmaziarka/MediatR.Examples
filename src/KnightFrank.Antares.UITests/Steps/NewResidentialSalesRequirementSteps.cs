namespace KnightFrank.Antares.UITests.Steps
{
    using System;

    using KnightFrank.Antares.UITests.Pages;

    using Objectivity.Test.Automation.Common;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class NewResidentialSalesRequirementSteps
    {
        private readonly DriverContext driverContext;
        private readonly ScenarioContext scenarioContext;


        public NewResidentialSalesRequirementSteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }

            this.scenarioContext = scenarioContext;
            this.driverContext = this.scenarioContext["DriverContext"] as DriverContext;
        }

        [Given(@"User navigates to create residential sales requirement page")]
        public void OpenNewResidentialSalesRequirementPage()
        {
            NewResidentialSalesRequirementPage newResidentialSalesRequirementPage = new NewResidentialSalesRequirementPage(this.driverContext).OpenNewResidentialSalesRequirementPage();
            this.scenarioContext["NewResidentialSalesRequirementPage"] = newResidentialSalesRequirementPage;
        }

        [When(@"User fills in location details on create residential sales requirement page")]
        public void SetLocationRequirementDetails(Table table)
        {
            var newResidentialSalesRequirementPage =
                this.scenarioContext.Get<NewResidentialSalesRequirementPage>("NewResidentialSalesRequirementPage");

            var details = table.CreateInstance<ResidentialSalesRequirementLocation>();

            newResidentialSalesRequirementPage.SetLocationStreetName(details.StreetName);
            newResidentialSalesRequirementPage.SetLocationPostCode(details.Postcode);
            newResidentialSalesRequirementPage.SetLocationCity(details.Town);
        }

        [When(@"User fills in property details on create residential sales requirement page")]
        public void SetPropertyRequirementDetails(Table table)
        {
            var newResidentialSalesRequirementPage =
                this.scenarioContext.Get<NewResidentialSalesRequirementPage>("NewResidentialSalesRequirementPage");

            var details = table.CreateInstance<ResidentialSalesRequirementProperty>();

                newResidentialSalesRequirementPage.SelectPropertyType(details.Type);
                newResidentialSalesRequirementPage.SetPropertyMinPrice(details.PriceMin);
                newResidentialSalesRequirementPage.SetPropertyMaxPrice(details.PriceMax);
                newResidentialSalesRequirementPage.SetPropertyBedroomsMin(details.BedroomsMin);
                newResidentialSalesRequirementPage.SetPropertyBedroomMax(details.BedroomsMax);
                newResidentialSalesRequirementPage.SetPropertyReceptionRoomsMin(details.ReceptionRoomsMin);
                newResidentialSalesRequirementPage.SetPropertyReceptionRoomsMax(details.ReceptionRoomsMax);
                newResidentialSalesRequirementPage.SetPropertyBathroomsMin(details.BathroomsMin);
                newResidentialSalesRequirementPage.SetPropertyBathroomsMax(details.BathroomsMax);
                newResidentialSalesRequirementPage.SetPropertyParkingSpacesMin(details.ParkingSpacesMin);
                newResidentialSalesRequirementPage.SetPropertyParkingSpacesMax(details.ParkingSpacesMax);
                newResidentialSalesRequirementPage.SetPropertyAreaMin(details.AreaMin);
                newResidentialSalesRequirementPage.SetPropertyAreaMax(details.AreaMax);
                newResidentialSalesRequirementPage.SetPropertyLandAreaMin(details.LandAreaMin);
                newResidentialSalesRequirementPage.SetPropertyLandAreaMax(details.LandAreaMax);
                newResidentialSalesRequirementPage.SetPropertyRequirementsNote(details.RequirementsNote);
        }

        [When(@"User clicks save button on create residential sales requirement page")]
        public void SaveNewResidentialSalesRequirement()
        {
            this.scenarioContext.Get<NewResidentialSalesRequirementPage>("NewResidentialSalesRequirementPage").SaveNewResidentialSalesRequirement();
        }

        [Then(@"New residential sales requirement should be created")]
        public void CheckIfNewResidentialSalesRequirementCreated()
        {
            //
        }
    }

    internal class ResidentialSalesRequirementLocation
    {
        public string Country { get; set; }

        public string StreetName { get; set; }

        public string Postcode { get; set; }

        public string Town { get; set; }
    }

    internal class ResidentialSalesRequirementProperty
    {
        public string Type { get; set; }

        public string PriceMin { get; set; }

        public string PriceMax { get; set; }

        public string BedroomsMin { get; set; }

        public string BedroomsMax { get; set; }

        public string ReceptionRoomsMin { get; set; }

        public string ReceptionRoomsMax { get; set; }

        public string BathroomsMin { get; set; }

        public string BathroomsMax { get; set; }

        public string ParkingSpacesMin { get; set; }

        public string ParkingSpacesMax { get; set; }

        public string AreaMin { get; set; }

        public string AreaMax { get; set; }

        public string LandAreaMin { get; set; }

        public string LandAreaMax { get; set; }

        public string RequirementsNote { get; set; }
    }
}
