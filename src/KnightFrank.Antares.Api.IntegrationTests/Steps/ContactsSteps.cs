namespace KnightFrank.Antares.Api.IntegrationTests.Steps
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;

    using FluentAssertions;

    using Fixtures;

    using Dal.Model;

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

        [Given(@"User has defined a contact details")]
        public void GetContactId(Table table)
        {
            var contact = table.CreateInstance<Contact>();
            this.fixture.DataContext.Contact.Add(contact);
            this.fixture.DataContext.SaveChanges();
        }

        [When(@"User retrieves '(all|specific)' contacts details")]
        public void GetContactsDetails(string getSpecificOrAllContacts)
        {
            string requestUrl = getSpecificOrAllContacts.Equals("all")
                ? $"{ApiUrl}"
                : $"{ApiUrl}/{this.fixture.DataContext.Contact.First().Id}";

            HttpResponseMessage response = this.fixture.Server.HttpClient.GetAsync(requestUrl).Result;

            ScenarioContext.Current.Set(response, "Response");
        }

        [Then(@"contacts should have following details")]
        public void CheckContactsDetails(Table table)
        {
            var response = ScenarioContext.Current.Get<HttpResponseMessage>("Response");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            List<Contact> expectedContactsDetails = table.CreateSet<Contact>().ToList();
            var currentContactsDetails = JsonConvert.DeserializeObject<List<Contact>>(response.Content.ReadAsStringAsync().Result);

            currentContactsDetails.ShouldBeEquivalentTo(expectedContactsDetails);
        }

        [Then(@"contact should have following details")]
        public void CheckContactDetails(Table table)
        {
            var response = ScenarioContext.Current.Get<HttpResponseMessage>("Response");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var contact = table.CreateInstance<Contact>();
            var currentContactDetails = JsonConvert.DeserializeObject<Contact>(response.Content.ReadAsStringAsync().Result);

            currentContactDetails.ShouldBeEquivalentTo(contact);
        }
    }
}
