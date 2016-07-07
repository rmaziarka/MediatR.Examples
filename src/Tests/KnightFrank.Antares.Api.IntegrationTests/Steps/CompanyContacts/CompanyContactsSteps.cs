namespace KnightFrank.Antares.Api.IntegrationTests.Steps.CompanyContacts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    using FluentAssertions;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Dal.Model.Company;

    using Newtonsoft.Json;

    using TechTalk.SpecFlow;

    using Xunit;

    [Binding]
    public class CompanyContactsSteps : IClassFixture<BaseTestClassFixture>
    {
        private const string ApiUrl = "/api/CompanyContacts";
        private readonly BaseTestClassFixture fixture;

        private readonly ScenarioContext scenarioContext;

        public CompanyContactsSteps(BaseTestClassFixture fixture, ScenarioContext scenarioContext)
        {
            this.fixture = fixture;
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            this.scenarioContext = scenarioContext;
        }

        [When(@"User retrieves company contacts")]
        public void GetCompanyContacts()
        {
            this.GetCompanyContactsRequest();
        }

        [Then(@"Company contacts details should have expected values")]
        public void CheckCompanyContacts()
        {
            var response = JsonConvert.DeserializeObject<List<CompanyContact>>(this.scenarioContext.GetResponseContent());
            List<CompanyContact> expectedCompanyContacts = this.fixture.DataContext.CompanyContacts.ToList();

            response.ShouldAllBeEquivalentTo(expectedCompanyContacts,
                opt => opt.Excluding(c => c.Company).Excluding(c => c.Contact));
        }

        private void GetCompanyContactsRequest()
        {
            string requestUrl = $"{ApiUrl}";
            HttpResponseMessage response = this.fixture.SendGetRequest(requestUrl);
            this.scenarioContext.SetHttpResponseMessage(response);
        }
    }
}
