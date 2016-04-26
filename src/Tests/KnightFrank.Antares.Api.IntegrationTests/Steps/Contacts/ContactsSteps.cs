namespace KnightFrank.Antares.Api.IntegrationTests.Steps.Contacts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    using FluentAssertions;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Dal.Model.Contacts;

    using Newtonsoft.Json;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    using Xunit;

    [Binding]
    public class ContactsSteps : IClassFixture<BaseTestClassFixture>
    {
        private const string ApiUrl = "/api/contacts";
        private readonly BaseTestClassFixture fixture;

        private readonly ScenarioContext scenarioContext;

        public ContactsSteps(BaseTestClassFixture fixture, ScenarioContext scenarioContext)
        {
            this.fixture = fixture;
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            this.scenarioContext = scenarioContext;
        }

        [Given(@"All contacts have been deleted")]
        public void DeleteContacts()
        {
            this.fixture.DataContext.Contacts.RemoveRange(this.fixture.DataContext.Contacts.ToList());
        }

        [Given(@"User creates contacts in database with following data")]
        public void CreateUsersInDb(Table table)
        {
            IEnumerable<Contact> contact = table.CreateSet<Contact>().ToList();
            this.fixture.DataContext.Contacts.AddRange(contact);
            this.fixture.DataContext.SaveChanges();

            this.scenarioContext.Set(contact, "Contact List");
        }

        [When(@"User creates contact using api with max lenght fields")]
        public void CreateUsersWithMaxFields()
        {
            const int max = 128;

            var contact = new Contact
            {
                FirstName = StringExtension.GenerateMaxAlphanumericString(max),
                Surname = StringExtension.GenerateMaxAlphanumericString(max),
                Title = StringExtension.GenerateMaxAlphanumericString(max)
            };

            this.CreateContact(contact);
        }

        [When(@"Try to creates a contact with following data")]
        public void TryToCreateContact(Table table)
        {
            string requestUrl = $"{ApiUrl}";
            var contact = table.CreateInstance<Contact>();
            HttpResponseMessage response = this.fixture.SendPostRequest(requestUrl, contact);

            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [When(@"User retrieves all contact details")]
        public void GetAllContactsDetails()
        {
            string requestUrl = $"{ApiUrl}";
            HttpResponseMessage response = this.fixture.SendGetRequest(requestUrl);

            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [When(@"User retrieves contact details for (.*) id")]
        public void GetContactDetailsUsingId(string contactId)
        {
            if (contactId.Equals("latest"))
            {
                contactId = this.scenarioContext.GetResponseContent().Replace("\"", string.Empty);

                var contacts = this.scenarioContext.Get<List<Contact>>("Contact List");
                contacts.First().Id = new Guid(contactId);
                this.scenarioContext["Contact List"] = contacts;
            }

            string requestUrl = $"{ApiUrl}/" + contactId;
            HttpResponseMessage response = this.fixture.SendGetRequest(requestUrl);

            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [Then(@"contact details should be the same as already added")]
        public void CheckContactDetails()
        {
            var expectedContact = this.scenarioContext.Get<Contact>("Contact");
            var id = new Guid(this.scenarioContext.GetResponseContent().Replace("\"", ""));

            Contact contact = this.fixture.DataContext.Contacts.Single(x => x.Id.Equals(id));
            expectedContact.Id = id;
            contact.ShouldBeEquivalentTo(expectedContact);
        }

        [Then(@"contact details should have expected values")]
        public void CheckContactDetailsHaveExpectedValues()
        {
            var contactList = this.scenarioContext.Get<List<Contact>>("Contact List");

            var currentContactsDetails =
                JsonConvert.DeserializeObject<List<Contact>>(this.scenarioContext.GetResponseContent());
            currentContactsDetails.ShouldBeEquivalentTo(contactList);
        }

        private void CreateContact(Contact contact)
        {
            string requestUrl = $"{ApiUrl}";
            HttpResponseMessage response = this.fixture.SendPostRequest(requestUrl, contact);
            this.scenarioContext.SetHttpResponseMessage(response);
            this.scenarioContext.Set(contact, "Contact");
        }
    }
}
