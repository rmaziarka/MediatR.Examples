namespace KnightFrank.Antares.UITests.Steps
{
    using System;
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.UITests.Pages;

    using Objectivity.Test.Automation.Common;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

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

        [Given(@"User creates contacts on create contact page")]
        public void CreateContacts(Table table)
        {
            var page = this.scenarioContext.Get<CreateContactPage>("CreateContactPage");
            IEnumerable<Contact> contacts = table.CreateSet<Contact>();
            foreach (Contact contact in contacts)
            {
                page.SetTitle(contact.Title)
                    .SetFirstName(contact.FirstName)
                    .SetSurname(contact.Surname)
                    .SaveContact();
            }
        }
    }
}
