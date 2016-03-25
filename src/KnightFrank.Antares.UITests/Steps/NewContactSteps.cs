namespace KnightFrank.Antares.UITests.Steps
{
    using System;
    
    using KnightFrank.Antares.Dal.Model.Contacts;
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
        [When(@"User navigates to create contact page")]
        public void OpenNewContactPage()
        {
            NewContactPage page = new NewContactPage(this.driverContext).OpenNewContactPage();
            this.scenarioContext["NewContactPage"] = page;
        }

        [When(@"User fills in contact details on create contact page")]
        public void SetNewContactDetails(Table table)
        {
            var page = this.scenarioContext.Get<NewContactPage>("NewContactPage");
            var contactDetails = table.CreateInstance<Contact>();

            page.SetTitle(contactDetails.Title)
                .SetFirstName(contactDetails.FirstName)
                .SetSurname(contactDetails.Surname);
        }

        [When(@"User clicks save button on create contact page")]
        public void SaveNewContact()
        {
            this.scenarioContext.Get<NewContactPage>("NewContactPage").SaveNewContact();
        }

        [Then(@"New contact should be created")]
        public void CheckIfContactCreated()
        {
            //TODO implement check if contact was created
        }
    }
}
