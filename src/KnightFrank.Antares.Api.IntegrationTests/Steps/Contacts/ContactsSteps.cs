namespace KnightFrank.Antares.Api.IntegrationTests.Steps.Contacts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    using FluentAssertions;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Dal.Model;

    using Newtonsoft.Json;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    using Xunit;

    [Binding]
    public class ContactsSteps : IClassFixture<BaseTestClassFixture>
    {
        private const string ApiUrl = "/api/contact";
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
        public void GivenDeleteAllContacts()
        {
            this.fixture.DataContext.Contact.RemoveRange(this.fixture.DataContext.Contact.ToList());
        }

        [When(@"User creates a contact with following data")]
        public void WhenUserCreatesAContactWithFollowingData(Table table)
        {
            IEnumerable<Contact> contact = table.CreateSet<Contact>().ToList();
            this.fixture.DataContext.Contact.AddRange(contact);
            this.fixture.DataContext.SaveChanges();
            this.scenarioContext.Set(contact, "Contact List");
        }

        [When(@"Try to creates a contact with following data")]
        public void WhenTryToCreatesAContactWithFollowingData(Table table)
        {
            string requestUrl = $"{ApiUrl}";
            var contact = table.CreateInstance<Contact>();
            HttpResponseMessage response = this.fixture.SendPostRequest(requestUrl, contact);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [When(@"User retrieves all contact details")]
        public void WhenUserRetrivesAllContactsDetails()
        {
            string requestUrl = $"{ApiUrl}";
            HttpResponseMessage response = this.fixture.SendGetRequest(requestUrl);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [When(@"User retrieves contacts details for (.*) id")]
        public void WhenUserRetrievesContactsDetailsWith(string id)
        {
            if (id.Equals("latest"))
            {
                var contactList = this.scenarioContext.Get<List<Contact>>("Contact List");
                id = contactList.Last().Id.ToString();
            }

            string requestUrl = $"{ApiUrl}/" + id + "";

            HttpResponseMessage response = this.fixture.SendGetRequest(requestUrl);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [Then(@"contact details should be the same as already added")]
        public void ThenContactDetailsShouldBeTheSameAsAlreadyAdded()
        {
            var contactList = this.scenarioContext.Get<List<Contact>>("Contact List");
            if (contactList.Count > 1)
            {
                var currentContactsDetails =
                    JsonConvert.DeserializeObject<List<Contact>>(this.scenarioContext.GetResponseContent());
                currentContactsDetails.ShouldBeEquivalentTo(contactList);
            }
            else
            {
                var currentContactDetails = JsonConvert.DeserializeObject<Contact>(this.scenarioContext.GetResponseContent());
                currentContactDetails.ShouldBeEquivalentTo(contactList[0]);
            }
        }
    }
}
