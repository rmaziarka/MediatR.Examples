namespace KnightFrank.Antares.UITests.Steps.Database
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal;
    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.UITests.Extensions;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class ContactSteps
    {
        private readonly ScenarioContext scenarioContext;

        public ContactSteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            this.scenarioContext = scenarioContext;
        }

        [Given(@"Contacts are created in database")]
        public void CreateContactsInDb(Table table)
        {
            List<Contact> contacts = table.CreateSet<Contact>().ToList();
            KnightFrankContext dataContext = DatabaseExtensions.GetDataContext();

            dataContext.Contacts.AddRange(contacts);
            dataContext.CommitAndClose();
            this.scenarioContext.Set(contacts, "ContactsList");
        }
    }
}
