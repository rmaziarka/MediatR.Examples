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

        [Given(@"Company exists in database")]
        public void CreateCompanyInDatabase(Table table)
        {
            var company = table.CreateInstance<Company>();
            List<Guid> contactsIds = this.scenarioContext.Get<List<Contact>>("Contacts").Select(c => c.Id).ToList();

            company.CompaniesContacts = contactsIds.Select(id => new CompanyContact { ContactId = id }).ToList();

            this.fixture.DataContext.Companies.Add(company);
            this.fixture.DataContext.SaveChanges();

            this.scenarioContext.Set(company, "Company");
        }

        [When(@"User creates company using api")]
        public void CreateCompany(Table table)
        {
            var details = table.CreateInstance<CreateCompanyCommand>();
            this.CreateCompany(details);
        }

        [When(@"User creates company with mandatory fields using api")]
        public void CreateCompanyWithMandatoryFields()
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

            actualCompany.ShouldBeEquivalentTo(expectedCompany, opt => opt.Excluding(c => c.CompaniesContacts));
        }

        private void CreateCompany(CreateCompanyCommand company)
        {
            string requestUrl = $"{ApiUrl}";
            var contactList = this.scenarioContext.Get<List<Contact>>("Contacts");
            company.ContactIds = contactList.Select(x => x.Id).ToList();

            this.scenarioContext.Set(new Company {Name = company.Name}, "Company");
            HttpResponseMessage response = this.fixture.SendPostRequest(requestUrl, company);
            this.scenarioContext.SetHttpResponseMessage(response);
        }
    }
}
