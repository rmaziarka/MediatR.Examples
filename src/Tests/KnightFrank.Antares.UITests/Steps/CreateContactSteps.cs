namespace KnightFrank.Antares.UITests.Steps
{
    using System;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.UITests.Pages;

    using Objectivity.Test.Automation.Common;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    using Xunit;

    [Binding]
    public class CreateContactSteps
    {
        private readonly DriverContext driverContext;
        private readonly ScenarioContext scenarioContext;

        public CreateContactSteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            this.scenarioContext = scenarioContext;
            this.driverContext = this.scenarioContext["DriverContext"] as DriverContext;
        }

        [Given(@"User navigates to create contact page")]
        public void OpenCreateContactPage()
        {
            CreateContactPage page = new CreateContactPage(this.driverContext).OpenCreateContactPage();
            this.scenarioContext["CreateContactPage"] = page;
        }

        [When(@"User fills in contact details on create contact page")]
        public void CreateContact(Table table)
        {
            var page = this.scenarioContext.Get<CreateContactPage>("CreateContactPage");
            var contact = table.CreateInstance<Contact>();

            page.SetTitle(contact.Title)
                .SetFirstName(contact.FirstName)
                .SetSurname(contact.Surname);
        }

        [When(@"User clicks save button on create contact page")]
        public void SaveContact()
        {
            this.scenarioContext.Get<CreateContactPage>("CreateContactPage").SaveContact();
        }

        [Then(@"New contact should be created")]
        public void CheckIfContactCreated()
        {
            //TODO implement check if contact was created
        }

        [Then(@"Contact form on create contact page should be displayed")]
        public void CheckIfFirstNameIsDisplayed()
        {
            Assert.True(new CreateContactPage(this.driverContext).IsContactFormPresent());
        }
    }
}
