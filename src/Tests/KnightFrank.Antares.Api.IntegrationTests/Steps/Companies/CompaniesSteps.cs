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
    using KnightFrank.Antares.Dal.Model.User;
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
        public void GivenCompanyExistsInDatabase()
        {
            const int max = 4000;
            var company = new Company { Name = StringExtension.GenerateMaxAlphanumericString(20) };
            List<Guid> contactsIds = this.scenarioContext.Get<List<Contact>>("Contacts").Select(c => c.Id).ToList();

            company.CompaniesContacts = contactsIds.Select(id => new CompanyContact { ContactId = id }).ToList();
            company.ClientCarePageUrl = "www.clientcare.com";
            company.WebsiteUrl = "www.api.com";
            company.ClientCareStatusId =
                this.fixture.DataContext.EnumTypeItems.Single(
                    eti => eti.EnumType.Code.Equals("ClientCareStatus") && eti.Code.Equals("MassiveActionClient")).Id;

            company.Description = StringExtension.GenerateMaxAlphanumericString(max);
            company.CompanyTypeId =
                this.fixture.DataContext.EnumTypeItems.Single(
                    eti => eti.EnumType.Code.Equals("CompanyType") && eti.Code.Equals("KnightFrankAffiliates")).Id;
            company.CompanyCategoryId =
                this.fixture.DataContext.EnumTypeItems.Single(
                    eti => eti.EnumType.Code.Equals("CompanyCategory") && eti.Code.Equals("Banks")).Id;
            company.Valid = true;
            company.RelationshipManagerId = this.scenarioContext.Get<List<User>>("User List").First().Id;

            this.fixture.DataContext.Companies.Add(company);
            this.fixture.DataContext.SaveChanges();

            this.scenarioContext.Set(company, "Company");
        }

        [When(@"User creates company with invalid (name|status|contact|company type|category|relationship) using api")]
        public void CreateCompanyWithInvalidDataUsingApi(string data)
        {
            var company = new CreateCompanyCommand
            {
                ContactIds =
                    data.Equals("contact")
                        ? new List<Guid> { Guid.NewGuid() }
                        : new List<Guid>(this.scenarioContext.Get<List<Contact>>("Contacts").Select(c => c.Id)),
                Name = data.Equals("name") ? string.Empty : StringExtension.GenerateMaxAlphanumericString(20),
                ClientCareStatusId =
                    data.Equals("status")
                        ? Guid.NewGuid()
                        : this.fixture.DataContext.EnumTypeItems.Single(
                            eti => eti.EnumType.Code.Equals("ClientCareStatus") && eti.Code.Equals("KeyClient")).Id,
                CompanyTypeId = data.Equals("company type")
                    ? Guid.NewGuid()
                    : this.fixture.DataContext.EnumTypeItems.Single(
                        eti => eti.EnumType.Code.Equals("CompanyType") && eti.Code.Equals("KnightFrankGroup")).Id,
                CompanyCategoryId = data.Equals("category")
                    ? Guid.NewGuid()
                    : this.fixture.DataContext.EnumTypeItems.Single(
                        eti => eti.EnumType.Code.Equals("CompanyCategory") && eti.Code.Equals("OilAndGas")).Id,
                RelationshipManagerId =
                    data.Equals("relationship") ? Guid.NewGuid() : this.scenarioContext.Get<List<User>>("User List").First().Id
            };

            this.CreateCompany(company);
        }

        [When(@"User creates company using api")]
        public void CreateCompanyUsingApi(Table table)
        {
            const int max = 4000;
            var company = table.CreateInstance<CreateCompanyCommand>();

            company.ClientCareStatusId =
                this.fixture.DataContext.EnumTypeItems.Single(
                    eti => eti.EnumType.Code.Equals("ClientCareStatus") && eti.Code.Equals("MassiveActionClient")).Id;
            company.ContactIds = new List<Guid>(this.scenarioContext.Get<List<Contact>>("Contacts").Select(c => c.Id));

            company.Description = StringExtension.GenerateMaxAlphanumericString(max);
            company.CompanyTypeId =
                this.fixture.DataContext.EnumTypeItems.Single(
                    eti => eti.EnumType.Code.Equals("CompanyType") && eti.Code.Equals("KnightFrankAffiliates")).Id;
            company.CompanyCategoryId =
                this.fixture.DataContext.EnumTypeItems.Single(
                    eti => eti.EnumType.Code.Equals("CompanyCategory") && eti.Code.Equals("Banks")).Id;
            company.Valid = true;
            company.RelationshipManagerId = this.scenarioContext.Get<List<User>>("User List").First().Id;

            this.CreateCompany(company);
        }

        [When(@"User creates company with required fields using api")]
        public void CreateCompanyRequiredFieldsUsingApi()
        {
            const int max = 128;
            var company = new CreateCompanyCommand
            {
                Name = StringExtension.GenerateMaxAlphanumericString(max),
                ContactIds = new List<Guid>(this.scenarioContext.Get<List<Contact>>("Contacts").Select(c => c.Id))
            };
            this.CreateCompany(company);
        }

        [When(@"User updates company using api")]
        public void UpdateCompany()
        {
            string requestUrl = $"{ApiUrl}";
            var company = this.scenarioContext.Get<Company>("Company");
            var contactList = this.scenarioContext.Get<List<Contact>>("Contacts");

            var commandCompany = new UpdateCompanyCommand
            {
                Id = company.Id,
                Name = StringExtension.GenerateMaxAlphanumericString(20),
                ClientCarePageUrl = "www.update.com",
                ClientCareStatusId = company.ClientCareStatusId,
                WebsiteUrl = "www.update.com2",
                Contacts = contactList.Select(c => new Contact { Id = c.Id }).ToList(),
                Description = StringExtension.GenerateMaxAlphanumericString(2000),
                CompanyTypeId =
                    this.fixture.DataContext.EnumTypeItems.Single(
                        eti => eti.EnumType.Code.Equals("CompanyType") && eti.Code.Equals("KnightFrankGroup")).Id,
                CompanyCategoryId =
                    this.fixture.DataContext.EnumTypeItems.Single(
                        eti => eti.EnumType.Code.Equals("CompanyCategory") && eti.Code.Equals("Supermarkets")).Id,
                Valid = false,
                RelationshipManagerId = this.fixture.DataContext.Users.First().Id
            };

            HttpResponseMessage response = this.fixture.SendPutRequest(requestUrl, commandCompany);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [When(@"User updates company with invalid (name|status|contact|company type|category|relationship) using api")]
        public void UpdateCompanyWithInvalidData(string data)
        {
            string requestUrl = $"{ApiUrl}";
            var company = this.scenarioContext.Get<Company>("Company");
            var contactList = this.scenarioContext.Get<List<Contact>>("Contacts");

            var commandCompany = new UpdateCompanyCommand
            {
                Id = company.Id,
                Name = data.Equals("name") ? string.Empty : StringExtension.GenerateMaxAlphanumericString(20),
                ClientCarePageUrl = company.ClientCarePageUrl,
                ClientCareStatusId = data.Equals("status") ? Guid.NewGuid() : company.ClientCareStatusId,
                WebsiteUrl = company.WebsiteUrl,
                Contacts =
                    data.Equals("contact")
                        ? new List<Contact> { new Contact { Id = Guid.NewGuid() } }
                        : contactList.Select(c => new Contact { Id = c.Id }).ToList(),
                CompanyTypeId = data.Equals("company type")
                    ? Guid.NewGuid()
                    : this.fixture.DataContext.EnumTypeItems.Single(
                        eti => eti.EnumType.Code.Equals("CompanyType") && eti.Code.Equals("KnightFrankGroup")).Id,
                CompanyCategoryId = data.Equals("category")
                    ? Guid.NewGuid()
                    : this.fixture.DataContext.EnumTypeItems.Single(
                        eti => eti.EnumType.Code.Equals("CompanyCategory") && eti.Code.Equals("OilAndGas")).Id,
                RelationshipManagerId =
                    data.Equals("relationship") ? Guid.NewGuid() : this.scenarioContext.Get<List<User>>("User List").First().Id
            };

            HttpResponseMessage response = this.fixture.SendPutRequest(requestUrl, commandCompany);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [When(@"User gets company details")]
        public void GetCompany()
        {
            Guid companyId = this.scenarioContext.ContainsKey("Company")
                ? this.scenarioContext.Get<Company>("Company").Id
                : Guid.NewGuid();

            this.GetCompany($"{ApiUrl}/{companyId}");
        }

        [When(@"User gets company details with invalid query")]
        public void GetCompanyInvalidQuery()
        {
            this.GetCompany($"{ApiUrl}/{Guid.Empty}");
        }

        [Then(@"Company should be same as in database")]
        [Then(@"Company should be updated")]
        public void CheckCompany()
        {
            var actualCompany = JsonConvert.DeserializeObject<Company>(this.scenarioContext.GetResponseContent());
            Company expectedCompany = this.fixture.DataContext.Companies.Single(c => c.Id.Equals(actualCompany.Id));

            expectedCompany.ShouldBeEquivalentTo(actualCompany,
                opt =>
                    opt.Excluding(c => c.ClientCareStatus)
                       .Excluding(c => c.CompaniesContacts)
                       .Excluding(c => c.CompanyCategory)
                       .Excluding(c => c.CompanyType)
                       .Excluding(c => c.RelationshipManager));

            expectedCompany.CompaniesContacts.Should()
                           .Equal(actualCompany.CompaniesContacts,
                               (c1, c2) =>
                                   c1.CompanyId.Equals(c2.CompanyId) && c1.ContactId.Equals(c2.ContactId) && c1.Id.Equals(c2.Id));
        }

        private void CreateCompany(CreateCompanyCommand company)
        {
            string requestUrl = $"{ApiUrl}";

            this.scenarioContext.Set(new Company
            {
                Name = company.Name,
                ClientCarePageUrl = company.ClientCarePageUrl,
                ClientCareStatusId = company.ClientCareStatusId,
                WebsiteUrl = company.WebsiteUrl
            }, "Company");

            HttpResponseMessage response = this.fixture.SendPostRequest(requestUrl, company);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        private void GetCompany(string requestUrl)
        {
            HttpResponseMessage response = this.fixture.SendGetRequest(requestUrl);
            this.scenarioContext.SetHttpResponseMessage(response);
        }
    }
}
