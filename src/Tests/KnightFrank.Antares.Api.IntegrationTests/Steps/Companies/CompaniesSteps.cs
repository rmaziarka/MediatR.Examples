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

        [Given(@"Company does not exist")]
        public void GivenCompanyDoesNotExist()
        {
            this.SetScenarioContextForAddedCompanyId(Guid.NewGuid());
        }

        [Given(@"Company exists in database")]
        public void GivenCompanyExistsInDatabase()
        {
           Company company = new Company() {Name = StringExtension.GenerateMaxAlphanumericString(20)};

            this.fixture.DataContext.Companies.Add(company);
            this.fixture.DataContext.SaveChanges();
            this.scenarioContext.Set(company, "Company");
            this.SetScenarioContextForAddedCompanyId(company.Id);
        }

		[Given(@"User creates company in database with following data")]
		public void CreateCompanyInDb(Table table)
		{
			Company company = table.CreateSet<Company>().Single();
			this.fixture.DataContext.Companies.Add(company);
			this.fixture.DataContext.SaveChanges();

			this.scenarioContext.Set(company, "Company");
		}

		[When(@"User creates company by API for contact")]
        public void WhenUserCreateCompanyByApiForContact(Table table)
        {
            string clientCareStatus =  this.GetClientCareStatus(table, "EnumTypeItemCode");
           
            Guid clientcareStatusId = this.scenarioContext.Get<Dictionary<string, Guid>>("EnumDictionary")[clientCareStatus];

            var company = table.CreateInstance<CreateCompanyCommand>();
            company.ClientCareStatusId = clientcareStatusId;

            this.CreateCompany(company);            
        }
    
        [When(@"User creates company by API with all fields")]
        public void WhenUserCreatesCompanyByApiWithAllFields(Table table)
        {
            string clientCareStatus = this.GetClientCareStatus(table, "ClientCareStatus");
            Guid clientcareStatusId = this.scenarioContext.Get<Dictionary<string, Guid>>("EnumDictionary")[clientCareStatus];
                  
            CreateCompanyCommand company = table.CreateInstance<CreateCompanyCommand>();
            company.ClientCareStatusId = clientcareStatusId;

            this.CreateCompany(company);
        }

        [When(@"User creates company by API for contact for maximum name length")]
        public void CreateUsersWithMaxFields()
        {
            const int max = 128;
            var company = new CreateCompanyCommand { Name = StringExtension.GenerateMaxAlphanumericString(max) };
            this.CreateCompany(company);
            
        }

		[When(@"User updates company by API")]
		public void UpdateCompany()
		{
			string requestUrl = $"{ApiUrl}";
			var company = this.scenarioContext.Get<Company>("Company");
			var contactList = this.scenarioContext.Get<List<Contact>>("ContactList");

			var commandCompany = new UpdateCompanyCommand
			{
				Id = company.Id,
				Name = company.Name,
				ClientCarePageUrl = company.ClientCarePageUrl,
				ClientCareStatusId = company.ClientCareStatusId,
				WebsiteUrl = company.WebsiteUrl,
				Contacts = contactList 
			};

			HttpResponseMessage response = this.fixture.SendPutRequest(requestUrl, commandCompany);
			this.scenarioContext.SetHttpResponseMessage(response);
		}

		[When(@"User updates company by API with invalid data")]
		public void UpdateCompanyWithInvalidData()
		{
			string requestUrl = $"{ApiUrl}";
			var company = this.scenarioContext.Get<Company>("Company");
			var contactList = this.scenarioContext.Get<List<Contact>>("ContactList");

			var commandCompany = new UpdateCompanyCommand
			{
				Id = company.Id,
				Name = string.Empty,
				ClientCarePageUrl = company.ClientCarePageUrl,
				ClientCareStatusId = company.ClientCareStatusId,
				WebsiteUrl = company.WebsiteUrl,
				Contacts = contactList
			};

			HttpResponseMessage response = this.fixture.SendPutRequest(requestUrl, commandCompany);
			this.scenarioContext.SetHttpResponseMessage(response);
		}

		[When(@"User gets company details")]
        public void WhenUserGetsCompanyDetails()
        {
            var companyId = this.scenarioContext.Get<Guid>("AddedCompanyId");
            string requestUrl = $"{ApiUrl}/{companyId}";

            HttpResponseMessage response = this.fixture.SendGetRequest(requestUrl);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [When(@"User gets company details with invalid query")]
        public void WhenUserGetsCompanyDetailsWithInvalidQuery()
        {
            string requestUrl = $"{ApiUrl}/{Guid.Empty}";

            HttpResponseMessage response = this.fixture.SendGetRequest(requestUrl);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [Then(@"Company should be added to database")]
       public void ThenCompanyShouldBeAddedToDataBase()
        {
            var company = JsonConvert.DeserializeObject<Company>(this.scenarioContext.GetResponseContent());
            var expectedCompany = this.scenarioContext.Get<Company>("Company");
            expectedCompany.Id = company.Id;
            Company actualCompany = this.fixture.DataContext.Companies.Single(x => x.Id.Equals(company.Id));

            actualCompany.ShouldBeEquivalentTo(expectedCompany, opt => opt
            .Excluding(c=>c.ClientCareStatus));
        }

		[Then(@"Company should be updated")]
		public void ThenCompanyShouldBeUpdatedInDataBase()
		{
			var company = JsonConvert.DeserializeObject<Company>(this.scenarioContext.GetResponseContent());
			var expectedCompany = this.scenarioContext.Get<Company>("Company");

			Company actualCompany = this.fixture.DataContext.Companies.Single(x => x.Id.Equals(company.Id));

			actualCompany.ShouldBeEquivalentTo(expectedCompany, opt => opt
						 .Excluding(c => c.ClientCareStatus));
		}

		[Then(@"Company details should match those in database")]
        public void ThenCompanyShouldMatchThoseInDatabase()
        {
            var actualCompany = JsonConvert.DeserializeObject<Company>(this.scenarioContext.GetResponseContent());

            var companyId = this.scenarioContext.Get<Guid>("AddedCompanyId");
            Company expectedCompany = this.fixture.DataContext.Companies.Single(x => x.Id.Equals(companyId));

            actualCompany.ShouldBeEquivalentTo(expectedCompany, opt => opt
            .Excluding(c => c.ClientCareStatus));
        }

        private void CreateCompany(CreateCompanyCommand company)
        {
            string requestUrl = $"{ApiUrl}";
            var contactList = this.scenarioContext.Get<List<Contact>>("ContactList");
           
            company.ContactIds = contactList.Select(x => x.Id).ToList();
            this.scenarioContext.Set(new Company
            {
                Name = company.Name,
                Contacts = contactList,
                ClientCarePageUrl = company.ClientCarePageUrl,
                ClientCareStatusId = company.ClientCareStatusId,
                WebsiteUrl = company.WebsiteUrl
            }, "Company");

            HttpResponseMessage response = this.fixture.SendPostRequest(requestUrl, company);
            this.scenarioContext.SetHttpResponseMessage(response);
        }
		private string GetClientCareStatus(Table table, string columName)
        {
            foreach (TableRow row in table.Rows)
            {
                return row[columName];
            }
            return string.Empty;
        }

        private void SetScenarioContextForAddedCompanyId(Guid id)
        {
            this.scenarioContext.Set(id, "AddedCompanyId");
        }
    }
}
