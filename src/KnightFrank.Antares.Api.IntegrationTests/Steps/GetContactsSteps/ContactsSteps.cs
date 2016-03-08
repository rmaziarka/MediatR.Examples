namespace KnightFrank.Antares.Api.IntegrationTests.Steps.GetContactsSteps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    using FluentAssertions;

    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Api.IntegrationTests.SharedActions;
    using KnightFrank.Antares.Dal.Model;

    using Newtonsoft.Json;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    using Xunit;

    [Binding]
    public class ContactsControllerSteps : IClassFixture<BaseTestClassFixture>
    {
        private const string ApiUrl = "/api/contacts";
        private readonly BaseTestClassFixture fixture;

        public ContactsControllerSteps(BaseTestClassFixture fixture)
        {
            this.fixture = fixture;
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
                PostRequest.SendPostRequest(this.fixture, requestUrl, contactRow);
                contactRow.Id = new Guid(ScenarioContext.Current.GetResponseContent().Replace("\"", ""));
                list.Add(contactRow);
        }
            ScenarioContext.Current.Set<List<Contact>>(list, "Contact List");
        }


        [When(@"User retrieves all contacts details")]
        public void WhenUserRetrivesAllContactsDetails()
        {
            string requestUrl = $"{ApiUrl}";
            GetRequest.SendGetRequest(this.fixture, requestUrl);
        }

        [When(@"User retrieves contacts details for (.*) id")]
        public void WhenUserRetrievesContactsDetailsWith(string id)
        {
            if (id.Equals("latest"))
            {
                id = ScenarioContext.Current.GetResponseContent().Replace("\"", "");
            }

            string requestUrl = $"{ApiUrl}/" + id + "";
            GetRequest.SendGetRequest(this.fixture, requestUrl);
        }

        [Then(@"User should get (.*) http status code")]
        public void ThenStatusCodeShouldBe(HttpStatusCode statusCode)
        {
            ScenarioContext.Current.GetResponseHttpStatusCode().Should().Be(statusCode);
        }


        [Then(@"contact details should be the same as already added")]
        public void ThenContactDetailsShouldBeTheSameAsAlreadyAdded()
        {
            var contactList = ScenarioContext.Current.Get<List<Contact>>("Contact List");
            if (contactList.Count > 1)
            {
                var currentContactsDetails =
                    JsonConvert.DeserializeObject<List<Contact>>(ScenarioContext.Current.GetResponseContent());
                currentContactsDetails.ShouldBeEquivalentTo(contactList);
        }
            else
        {
            var currentContactDetails = JsonConvert.DeserializeObject<Contact>(ScenarioContext.Current.GetResponseContent());
                currentContactDetails.ShouldBeEquivalentTo(contactList[0]);
        }

        }
    }
}
