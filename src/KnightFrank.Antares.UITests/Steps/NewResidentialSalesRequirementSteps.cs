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

        public NewResidentialSalesRequirementSteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }

            ScenarioContext sc = scenarioContext;
            this.driverContext = sc["DriverContext"] as DriverContext;
        }

        [Given(@"User navigates to create residential sales requirement page")]
        public void OpenNewResidentialSalesRequirementPage()
        {
            NewResidentialSalesRequirementPage newResidentialSalesRequirementPage = new NewResidentialSalesRequirementPage(this.driverContext).OpenNewResidentialSalesRequirementPage();
            ScenarioContext.Current["NewResidentialSalesRequirementPage"] = newResidentialSalesRequirementPage;
        }

        [When(@"User fills in location details on create residential sales requirement page")]
        public void SetLocationRequirementDetails(Table table)
        {
            var newResidentialSalesRequirementPage =
                ScenarioContext.Current.Get<NewResidentialSalesRequirementPage>("NewResidentialSalesRequirementPage");

            var details = table.CreateInstance<ResidentialSalesRequirementLocation>();

            if (!details.StreetName.Equals(string.Empty))
                newResidentialSalesRequirementPage.SetLocationStreetName(details.StreetName);

            if (!details.Postcode.Equals(string.Empty))
                newResidentialSalesRequirementPage.SetLocationPostCode(details.Postcode);

            if (!details.Town.Equals(string.Empty))
                newResidentialSalesRequirementPage.SetLocationCity(details.Town);
        }

        [When(@"User fills in property details on create residential sales requirement page")]
        public void SetPropertyRequirementDetails(Table table)
        {
            var newResidentialSalesRequirementPage =
                ScenarioContext.Current.Get<NewResidentialSalesRequirementPage>("NewResidentialSalesRequirementPage");

            var details = table.CreateInstance<ResidentialSalesRequirementProperty>();

            if (!details.Type.Equals(string.Empty))
                newResidentialSalesRequirementPage.SelectPropertyType(details.Type);

            if (!details.PriceMin.Equals(string.Empty))
                newResidentialSalesRequirementPage.SetPropertyMinPrice(details.PriceMin);

            if (!details.PriceMax.Equals(string.Empty))
                newResidentialSalesRequirementPage.SetPropertyMaxPrice(details.PriceMax);

            if (!details.BedroomsMin.Equals(string.Empty))
                newResidentialSalesRequirementPage.SetPropertyBedroomsMin(details.BedroomsMin);

            if (!details.BedroomsMax.Equals(string.Empty))
                newResidentialSalesRequirementPage.SetPropertyBedroomMax(details.BedroomsMax);

            if (!details.ReceptionRoomsMin.Equals(string.Empty))
                newResidentialSalesRequirementPage.SetPropertyReceptionRoomsMin(details.ReceptionRoomsMin);

            if (!details.ReceptionRoomsMax.Equals(string.Empty))
                newResidentialSalesRequirementPage.SetPropertyReceptionRoomsMax(details.ReceptionRoomsMax);

            if (!details.BathroomsMin.Equals(string.Empty))
                newResidentialSalesRequirementPage.SetPropertyBathroomsMin(details.BathroomsMin);

            if (!details.BathroomsMax.Equals(string.Empty))
                newResidentialSalesRequirementPage.SetPropertyBathroomsMax(details.BathroomsMax);

            if (!details.ParkingSpacesMin.Equals(string.Empty))
                newResidentialSalesRequirementPage.SetPropertyParkingSpacesMin(details.ParkingSpacesMin);

            if (!details.ParkingSpacesMax.Equals(string.Empty))
                newResidentialSalesRequirementPage.SetPropertyParkingSpacesMax(details.ParkingSpacesMax);

            if (!details.AreaMin.Equals(string.Empty))
                newResidentialSalesRequirementPage.SetPropertyAreaMin(details.AreaMin);

            if (!details.AreaMax.Equals(string.Empty))
                newResidentialSalesRequirementPage.SetPropertyAreaMax(details.AreaMax);

            if (!details.LandAreaMin.Equals(string.Empty))
                newResidentialSalesRequirementPage.SetPropertyLandAreaMin(details.LandAreaMin);

            if (!details.LandAreaMax.Equals(string.Empty))
                newResidentialSalesRequirementPage.SetPropertyLandAreaMax(details.LandAreaMax);

            if (!details.RequirementsNote.Equals(string.Empty))
                newResidentialSalesRequirementPage.SetPropertyRequirementsNote(details.RequirementsNote);
        }

        [When(@"User clicks save button on create residential sales requirement page")]
        public void SaveNewResidentialSalesRequirement()
        {
            ScenarioContext.Current.Get<NewResidentialSalesRequirementPage>("NewResidentialSalesRequirementPage").SaveNewResidentialSalesRequirement();
        }

        [Then(@"New residential sales requirement should be created")]
        public void CheckIfNewResidentialSalesRequirementCreated()
        {
            ScenarioContext.Current.Pending();
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
