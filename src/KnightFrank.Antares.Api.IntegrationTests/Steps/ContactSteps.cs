namespace KnightFrank.Antares.Api.IntegrationTests.Steps
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;

    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Dal;

    using Newtonsoft.Json;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    using Xunit;

    [Binding]
    public class ContactControllerSteps : IClassFixture<BaseTestClassFixture>
    {
        private readonly BaseTestClassFixture fixture;

        private const string ApiUrl = "/api/contact";

        public ContactControllerSteps(BaseTestClassFixture fixture)
        {
            this.fixture = fixture;
        }

        [Given(@"User has defined a contact details")]
        public void GetContactId()
        {
            ScenarioContext.Current["ContactID"] = "1";
        }

        [When(@"User retrieves contact details")]
        public void GetContactDetails()
        {
            var contactId = ScenarioContext.Current.Get<string>("ContactID");
            string requestUrl = $"{ApiUrl}/{contactId}";

            HttpResponseMessage response = this.fixture.Server.HttpClient.GetAsync(requestUrl).Result;

            ScenarioContext.Current.Set(response, "Response");
        }

        [Given(@"User retrieves all contacts details")]
        public void GetContactsDetails()
        {
            string requestUrl = $"{ApiUrl}";

            HttpResponseMessage response = this.fixture.Server.HttpClient.GetAsync(requestUrl).Result;

            ScenarioContext.Current.Set(response, "Response");
        }

        [Then(@"contacts should have following details")]
        public void CheckContactsDetails(Table table)
        {
            List<Contact> expectedContactsDetails = table.CreateSet<Contact>().ToList();
            var response = ScenarioContext.Current.Get<HttpResponseMessage>("Response");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var currentContactsDetails = JsonConvert.DeserializeObject<List<Contact>>(response.Content.ReadAsStringAsync().Result);

            for (var i = 0; i < currentContactsDetails.Count; i++)
            {
                Assert.Equal(expectedContactsDetails[i].FirstName, currentContactsDetails[i].FirstName);
                Assert.Equal(expectedContactsDetails[i].Surname, currentContactsDetails[i].Surname);
            }
        }
        [Then(@"contact should have following details")]
        public void CheckContactDetails(Table table)
        {
            var expectedContactDetails = table.CreateInstance<Contact>();
            var response = ScenarioContext.Current.Get<HttpResponseMessage>("Response");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var currentContactDetails = JsonConvert.DeserializeObject<Contact>(response.Content.ReadAsStringAsync().Result);

            Assert.Equal(expectedContactDetails.FirstName, currentContactDetails.FirstName);
            Assert.Equal(expectedContactDetails.Surname, currentContactDetails.Surname);
        }
    }
}
