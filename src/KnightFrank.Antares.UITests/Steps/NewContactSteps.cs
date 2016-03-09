namespace KnightFrank.Antares.UITests.Steps
{
    using System;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.UITests.Pages;

    using Objectivity.Test.Automation.Common;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

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
            var contactDetails = table.CreateInstance<Contact>();

            if (!contactDetails.Title.Equals(string.Empty))
            {
                newContactPage.SetTitle(contactDetails.Title);
            }

            if (!contactDetails.FirstName.Equals(string.Empty))
            {
                newContactPage.SetFirstName(contactDetails.FirstName);
            }

            if (!contactDetails.Surname.Equals(string.Empty))
            {
                newContactPage.SetSurname(contactDetails.Surname);
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
            Console.Write("abc");
        }
    }
}
