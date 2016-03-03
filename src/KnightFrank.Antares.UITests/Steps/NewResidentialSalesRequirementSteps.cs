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

        [When(@"User fills in requirement details on create residential sales requirement page")]
        public void SetRequirementDetails(Table table)
        {
            var newResidentialSalesRequirementPage =
                ScenarioContext.Current.Get<NewResidentialSalesRequirementPage>("NewResidentialSalesRequirementPage");

            var details = table.CreateInstance<ResidentialSalesRequirement>();

            if (!details.StreetName.Equals(string.Empty))
            {
                newResidentialSalesRequirementPage.SetStreetName(details.StreetName);
            }

            if (!details.Postcode.Equals(string.Empty))
            {
                newResidentialSalesRequirementPage.SetPostCode(details.Postcode);
            }

            if (!details.Town.Equals(string.Empty))
            {
                newResidentialSalesRequirementPage.SetTown(details.Town);
            }
        }

        [When(@"User clicks save button on create residential sales requirement page")]
        public void SaveNewResidentialSalesRequirement()
        {
            ScenarioContext.Current.Get<NewResidentialSalesRequirementSteps>("NewContactPage").SaveNewResidentialSalesRequirement();
        }

        [Then(@"New residential sales requirement should be created")]
        public void CheckIfNewResidentialSalesRequirementCreated()
        {
            ScenarioContext.Current.Pending();
        }
    }

    internal class ResidentialSalesRequirement
    {
        public string Country { get; set; }

        public string StreetName { get; set; }

        public string Postcode { get; set; }

        public string Town { get; set; }
    }
}
