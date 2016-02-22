using System;
using KnightFrank.Antares.UITests.Pages;
using Objectivity.Test.Automation.Common;
using TechTalk.SpecFlow;

namespace KnightFrank.Antares.UITests.Steps
{
    [Binding]
    public class NewContactSteps
    {
        private readonly DriverContext driverContext;

        public NewContactSteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null) throw new ArgumentNullException(nameof(scenarioContext));
            var sc = scenarioContext;

            driverContext = sc["DriverContext"] as DriverContext;
        }

        [Given(@"User navigates to create contact page")]
        public void OpenNewContactPage()
        {
            var newContactPage = new NewContactPage(driverContext).OpenNewContactPage();
            ScenarioContext.Current["NewContactPage"] = newContactPage;
        }

        [When(@"User fills in contact details on create contact page")]
        public void SetNewContactDetails(Table table)
        {
            var newContactPage = ScenarioContext.Current.Get<NewContactPage>("NewContactPage");

            if (!table.Rows[0]["Title"].Equals(""))
            {
                newContactPage.SetTitle(table.Rows[0]["Title"]);
            }
            if (!table.Rows[0]["First Name"].Equals(""))
            {
                newContactPage.SetFirstName(table.Rows[0]["First Name"]);
            }
            if (!table.Rows[0]["Surname"].Equals(""))
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
