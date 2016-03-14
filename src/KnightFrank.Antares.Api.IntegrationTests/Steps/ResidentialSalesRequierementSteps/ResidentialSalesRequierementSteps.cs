namespace KnightFrank.Antares.Api.IntegrationTests.Steps.ResidentialSalesRequierementSteps
{
    using System;
    using System.Collections.Generic;
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
    public class ResidentialSalesRequierementSteps : IClassFixture<BaseTestClassFixture>
    {
        private const string ApiUrl = "/api/requirement";
        private readonly BaseTestClassFixture fixture;

        private readonly ScenarioContext scenarioContext;

        public ResidentialSalesRequierementSteps(BaseTestClassFixture fixture, ScenarioContext scenarioContext)
        {
            this.fixture = fixture;
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            this.scenarioContext = scenarioContext;
        }

        [When(@"User creates following requirement")]
        public void UserCreatesFollowingRequirement(Table table)
        {
            string requestUrl = $"{ApiUrl}";
            
            var contacts = this.scenarioContext.Get<List<Contact>>("Contact List");

            var requirement = table.CreateInstance<Requirement>();
            requirement.Contacts.Add(contacts[0]);
            HttpResponseMessage response = this.fixture.SendPostRequest(requestUrl, requirement);
            this.scenarioContext.SetHttpResponseMessage(response);
            this.scenarioContext.Set(requirement, "Requirement");
        }

        [When(@"User retrieves requirement that he saved")]
        public void WhenUserRetrievesContactsDetailsWith()
        {
            // TODO uncomment after requirement backend is ready
            // string id = this.scenarioContext.GetResponseContent().Replace("\"", "");

            // string requestUrl = $"{ApiUrl}/" + id + "";

            // HttpResponseMessage response = this.fixture.SendGetRequest(requestUrl);
            // this.scenarioContext.SetHttpResponseMessage(response);
        }

        [Then(@"Requirement should be the same as added")]
        public void ThenRequierementShouldBeTheSameAsAdded()
        {
            // TODO uncomment after requirement backend is ready
            // var requirement = this.scenarioContext.Get<Requirement>("Requirement");
            // addedRequirement = JsonConvert.DeserializeObject<Requirement>(this.scenarioContext.GetResponseContent());            
            // addedRequirement.ShouldBeEquivalentTo(requirement,
            //    opt => opt.Excluding(req => req.Id).Excluding(req => req.CreateDate));
        }
    }
}
