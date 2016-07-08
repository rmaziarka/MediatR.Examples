namespace KnightFrank.Antares.UITests.Steps.Database
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal;
    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Domain.Common.Enums;

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
        [Given(@"Contact is created in database")]
        [When(@"Contacts are created in database")]
        public void CreateContactsInDb(Table table)
        {
            List<Contact> contacts = table.CreateSet<Contact>().ToList();

            foreach (Contact contact in contacts)
            {
                contact.ContactUsers = new List<ContactUser>
                {
                    new ContactUser
                    {
                        UserId = this.dataContext.Users.ToList().First().Id,
                        UserTypeId =
                            this.dataContext.EnumTypeItems.Single(
                                e => e.EnumType.Code.Equals(nameof(UserType)) && e.Code.Equals(nameof(UserType.LeadNegotiator))).Id
                    },
                    new ContactUser
                    {
                        UserId = this.dataContext.Users.ToList().ElementAt(1).Id,
                        UserTypeId =
                            this.dataContext.EnumTypeItems.Single(
                                e =>
                                    e.EnumType.Code.Equals(nameof(UserType)) && e.Code.Equals(nameof(UserType.SecondaryNegotiator)))
                                .Id
                    },
                    new ContactUser
                    {
                        UserId = this.dataContext.Users.ToList().ElementAt(2).Id,
                        UserTypeId =
                            this.dataContext.EnumTypeItems.Single(
                                e =>
                                    e.EnumType.Code.Equals(nameof(UserType)) && e.Code.Equals(nameof(UserType.SecondaryNegotiator)))
                                .Id
                    }
                };

                contact.DefaultMailingSalutationId = this.dataContext.EnumTypeItems.Single(
                    e =>
                        e.EnumType.Code.Equals(nameof(MailingSalutation)) && e.Code.Equals(nameof(MailingSalutation.MailingFormal)))
                                                         .Id;
            }

            this.dataContext.Contacts.AddRange(contacts);
            this.dataContext.SaveChanges();

            this.scenarioContext.Set(contacts, "ContactsList");
        }
    }
}
