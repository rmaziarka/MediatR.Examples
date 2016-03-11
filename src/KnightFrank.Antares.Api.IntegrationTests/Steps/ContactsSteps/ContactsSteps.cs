namespace KnightFrank.Antares.Api.IntegrationTests.Steps.ContactsSteps
{
    using System;
    using System.Collections.Generic;
    using System.Net;
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
    public class ContactsControllerSteps : IClassFixture<BaseTestClassFixture>
    {
        private const string ApiUrl = "/api/contact";
        private readonly BaseTestClassFixture fixture;

        private readonly ScenarioContext scenarioContext;

        public ContactsControllerSteps(BaseTestClassFixture fixture, ScenarioContext scenarioContext)
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
            this.fixture.DataContext.Database.ExecuteSqlCommand(@"delete from [dbo].Contact");
        }

        [When(@"User creates a contact with following data")]
        public void WhenUserCreatesAContactWithFollowingData(Table table)
        {
            string requestUrl = $"{ApiUrl}";
            IEnumerable<Contact> contact = table.CreateSet<Contact>();
            var list = new List<Contact>();

            foreach (Contact contactRow in contact)
            {
                HttpResponseMessage response = this.fixture.SendPostRequest(requestUrl, contactRow);
                this.scenarioContext.SetHttpResponseMessage(response);
                contactRow.Id = new Guid(this.scenarioContext.GetResponseContent().Replace("\"", ""));
                list.Add(contactRow);
            }
            this.scenarioContext.Set(list, "Contact List");
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
                id = this.scenarioContext.GetResponseContent().Replace("\"", "");
            }

            string requestUrl = $"{ApiUrl}/" + id + "";

            HttpResponseMessage response = this.fixture.SendGetRequest(requestUrl);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [Then(@"User should get (.*) http status code")]
        public void ThenStatusCodeShouldBe(HttpStatusCode statusCode)
        {
            this.scenarioContext.GetResponseHttpStatusCode().Should().Be(statusCode);
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
