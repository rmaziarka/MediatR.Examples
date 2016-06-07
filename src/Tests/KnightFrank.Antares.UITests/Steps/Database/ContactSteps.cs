namespace KnightFrank.Antares.UITests.Steps.Database
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal;
    using KnightFrank.Antares.Dal.Model.Contacts;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class ContactSteps
    {
        private readonly KnightFrankContext dataContext;
        private readonly ScenarioContext scenarioContext;

        public ContactSteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            this.scenarioContext = scenarioContext;
            this.dataContext = this.scenarioContext.Get<KnightFrankContext>("DataContext");
        }

        [Given(@"Contacts are created in database")]
        [When(@"Contacts are created in database")]
        public void CreateContactsInDb(Table table)
        {
            List<Contact> contacts = table.CreateSet<Contact>().ToList();

            this.dataContext.Contacts.AddRange(contacts);
            this.dataContext.SaveChanges();

            this.scenarioContext.Set(contacts, "ContactsList");
        }
    }
}
