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
        private readonly ScenarioContext scenarioContext;

        public NewContactSteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            this.scenarioContext = scenarioContext;
            this.driverContext = this.scenarioContext["DriverContext"] as DriverContext;
        }

        [Given(@"User navigates to create contact page")]
        public void OpenNewContactPage()
        {
            NewContactPage newContactPage = new NewContactPage(this.driverContext).OpenNewContactPage();
            this.scenarioContext["NewContactPage"] = newContactPage;
        }

        [When(@"User fills in contact details on create contact page")]
        public void SetNewContactDetails(Table table)
        {
            var newContactPage = this.scenarioContext.Get<NewContactPage>("NewContactPage");
            var contactDetails = table.CreateInstance<Contact>();

            newContactPage.SetTitle(contactDetails.Title);
            newContactPage.SetFirstName(contactDetails.FirstName);
            newContactPage.SetSurname(contactDetails.Surname);
         
        }

        [When(@"User clicks save button on create contact page")]
        public void SaveNewContact()
        {
            this.scenarioContext.Get<NewContactPage>("NewContactPage").SaveNewContact();
        }

        [Then(@"New contact should be created")]
        public void CheckIfContactCreated()
        {
            Console.Write("abc");
        }
    }
}
