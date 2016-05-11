namespace KnightFrank.Antares.Api.IntegrationTests.Steps.Companies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    using FluentAssertions;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Dal.Model.Company;
    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Domain.Company.Commands;

    using Newtonsoft.Json;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class CompaniesSteps
    {
        private const string ApiUrl = "/api/companies";
        private readonly BaseTestClassFixture fixture;

        private readonly ScenarioContext scenarioContext;

        public CompaniesSteps(BaseTestClassFixture fixture, ScenarioContext scenarioContext)
        {
            this.fixture = fixture;
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            this.scenarioContext = scenarioContext;
        }

        [When(@"User creates company by API for contact")]
        public void WhenUserCreateCompanyByApiForContact(Table table)
        {
            var company = table.CreateInstance<CreateCompanyCommand>();
            this.CreateCompany(company);
        }

        [When(@"User creates company by API for contact for maximum name length")]
        public void CreateUsersWithMaxFields()
        {
            const int max = 128;
            var company = new CreateCompanyCommand { Name = StringExtension.GenerateMaxAlphanumericString(max) };
            this.CreateCompany(company);
        }

        [Then(@"Company should be added to database")]
        public void ThenCompanyShouldBeAddedToDataBase()
        {
            var company = JsonConvert.DeserializeObject<Company>(this.scenarioContext.GetResponseContent());
            var expectedCompany = this.scenarioContext.Get<Company>("Company");
            expectedCompany.Id = company.Id;
            Company actualCompany = this.fixture.DataContext.Companies.Single(x => x.Id.Equals(company.Id));

            actualCompany.ShouldBeEquivalentTo(expectedCompany);
        }

        private void CreateCompany(CreateCompanyCommand company)
        {
            string requestUrl = $"{ApiUrl}";
            var contactList = this.scenarioContext.Get<List<Contact>>("ContactList");
            company.ContactIds = contactList.Select(x => x.Id).ToList();

            this.scenarioContext.Set(new Company { Name = company.Name, Contacts = contactList }, "Company");
            HttpResponseMessage response = this.fixture.SendPostRequest(requestUrl, company);
            this.scenarioContext.SetHttpResponseMessage(response);
        }
    }
}
