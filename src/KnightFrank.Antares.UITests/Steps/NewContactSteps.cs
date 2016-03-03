namespace KnightFrank.Antares.UITests.Steps
{
    using System;

    using KnightFrank.Antares.UITests.Pages;

    using Objectivity.Test.Automation.Common;

    using TechTalk.SpecFlow;

    [Binding]
    public class NewContactSteps
    {
        private readonly DriverContext driverContext;

        public NewContactSteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }

            ScenarioContext sc = scenarioContext;
            this.driverContext = sc["DriverContext"] as DriverContext;
        }

        [Given(@"User navigates to create contact page")]
        public void OpenNewContactPage()
        {
            NewContactPage newContactPage = new NewContactPage(this.driverContext).OpenNewContactPage();
            ScenarioContext.Current["NewContactPage"] = newContactPage;
        }

        [When(@"User fills in contact details on create contact page")]
        public void SetNewContactDetails(Table table)
        {
            var newContactPage = ScenarioContext.Current.Get<NewContactPage>("NewContactPage");

            if (!table.Rows[0]["Title"].Equals(string.Empty))
            {
                newContactPage.SetTitle(table.Rows[0]["Title"]);
            }

            if (!table.Rows[0]["FirstName"].Equals(string.Empty))
            {
                newContactPage.SetFirstName(table.Rows[0]["FirstName"]);
            }

            if (!table.Rows[0]["Surname"].Equals(string.Empty))
            {
                newContactPage.SetSurname(table.Rows[0]["Surname"]);
            }
        }

        [When(@"User clicks save button on create contact page")]
        public void SaveNewContact()
        {
            ScenarioContext.Current.Get<NewContactPage>("NewContactPage").SaveNewContact();
        }

        [Then(@"New contact should be created")]
        public void CheckIfContactCreated()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
