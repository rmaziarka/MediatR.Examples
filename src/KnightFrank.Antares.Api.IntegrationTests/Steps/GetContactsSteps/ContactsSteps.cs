namespace KnightFrank.Antares.Api.IntegrationTests.Steps.GetContactsSteps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

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
        private Guid latestAddedContactId;

        public ContactsControllerSteps(BaseTestClassFixture fixture)
        {
            this.fixture = fixture;
        }

        [Given(@"User has defined a contact details")]
        public void GivenCreateContact(Table table)
        {
            var contact = table.CreateInstance<Contact>();
            this.fixture.DataContext.Database.ExecuteSqlCommand(@"DELETE FROM [dbo].Contact");
            this.fixture.DataContext.Contact.Add(contact);
            this.fixture.DataContext.SaveChanges();
            this.latestAddedContactId = contact.Id;
        }

        [Given(@"User has defined multiple contact details")]
        public void GivenCreateContacts(Table table)
        {
            IEnumerable<Contact> set = table.CreateSet<Contact>();
            this.fixture.DataContext.Database.ExecuteSqlCommand(@"DELETE FROM [dbo].Contact");
            this.fixture.DataContext.Contact.AddRange(set);
            this.fixture.DataContext.SaveChanges();
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
            if (id.Equals("proper"))
            {
                id = this.latestAddedContactId.ToString();
            }

            string requestUrl = $"{ApiUrl}/" + id + "";
            GetRequest.SendGetRequest(this.fixture, requestUrl);
        }

        [Then(@"User should get (.*) http status code")]
        public void ThenStatusCodeShouldBe(HttpStatusCode statusCode)
        {
            statusCode.Should().Be(ScenarioContext.Current.GetResponseHttpStatusCode());
        }

        [Then(@"contacts should have following details")]
        public void ThenContactsShouldHaveFollowingDetails(Table table)
        {
            List<Contact> expectedContactsDetails = table.CreateSet<Contact>().ToList();
            var currentContactsDetails = JsonConvert.DeserializeObject<List<Contact>>(ScenarioContext.Current.GetResponseContent());

            currentContactsDetails.ShouldAllBeEquivalentTo(expectedContactsDetails, options => options.Excluding(c => c.Id));
        }

        [Then(@"contact should have same details as inserted")]
        public void ThenContactShouldHaveFollowingDetails()
        {
            var currentContactDetails = JsonConvert.DeserializeObject<Contact>(ScenarioContext.Current.GetResponseContent());
            currentContactDetails.ShouldBeEquivalentTo(this.fixture.DataContext.Contact.First());
        }

        [Then(@"User should get (.*) error mesage")]
        public void ThenErrorMesageShouldBeBlaBla(string expectedResponse)
        {
            expectedResponse.Should().Be(ScenarioContext.Current.GetResponseContent());
        }
    }
}
