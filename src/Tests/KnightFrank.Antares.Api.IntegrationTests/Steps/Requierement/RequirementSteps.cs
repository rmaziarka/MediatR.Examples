namespace KnightFrank.Antares.Api.IntegrationTests.Steps.Requierement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    using FluentAssertions;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Domain.Requirement.Commands;

    using Newtonsoft.Json;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    using Xunit;

    [Binding]
    public class RequirementSteps : IClassFixture<BaseTestClassFixture>
    {
        private const string ApiUrl = "/api/requirements";
        private readonly BaseTestClassFixture fixture;

        private readonly ScenarioContext scenarioContext;

        public RequirementSteps(BaseTestClassFixture fixture, ScenarioContext scenarioContext)
        {
            this.fixture = fixture;
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            this.scenarioContext = scenarioContext;
        }

        [Given(@"User sets locations details for the requirement")]
        [When(@"User sets locations details for the requirement")]
        public void SetRequirementLocationDetails(Table table)
        {
            var details = table.CreateInstance<CreateOrUpdateRequirementAddress>();
            details.AddressFormId = this.scenarioContext.Get<Guid>("AddressFormId");
            details.CountryId = this.scenarioContext.Get<Guid>("CountryId");

            this.scenarioContext.Set(details, "Location");
        }

        [When(@"User sets locations details for the requirement with max length fields")]
        public void SetRequirementLocationDetailsWithMaxFields()
        {
            var details = new CreateOrUpdateRequirementAddress
            {
                City = StringExtension.GenerateMaxAlphanumericString(128),
                Line2 = StringExtension.GenerateMaxAlphanumericString(128),
                Postcode = StringExtension.GenerateMaxAlphanumericString(10),
                AddressFormId = this.scenarioContext.Get<Guid>("AddressFormId"),
                CountryId = this.scenarioContext.Get<Guid>("CountryId")
            };

            this.scenarioContext.Set(details, "Location");
        }

        [When(@"User creates following requirement in database")]
        [Given(@"User creates following requirement in database")]
        public void CreateRequirementWithInDb(Table table)
        {
            var requirement = table.CreateInstance<Requirement>();

            requirement.CreateDate = DateTime.Now;
            requirement.Contacts.AddRange(this.scenarioContext.Get<List<Contact>>("ContactList"));

            var location = this.scenarioContext.Get<CreateOrUpdateRequirementAddress>("Location");
            requirement.Address = new Address
            {
                Line2 = location.Line2,
                Postcode = location.Postcode,
                City = location.City,
                CountryId = location.CountryId,
                AddressFormId = location.AddressFormId
            };

            this.fixture.DataContext.Requirements.Add(requirement);
            this.fixture.DataContext.SaveChanges();

            this.scenarioContext.Set(requirement, "Requirement");
        }

        [When(@"User creates following requirement using api")]
        public void CreateRequirementWithApi(Table table)
        {
            string requestUrl = $"{ApiUrl}";

            var contacts = this.scenarioContext.Get<List<Contact>>("ContactList");
            var requirement = table.CreateInstance<CreateRequirementCommand>();

            requirement.CreateDate = DateTime.Now;
            requirement.ContactIds = contacts.Select(contact => contact.Id).ToList();
            
            requirement.Address = this.scenarioContext.Get<CreateOrUpdateRequirementAddress>("Location");
            if (requirement.Description.ToLower().Equals("max"))
            {
                requirement.Description = StringExtension.GenerateMaxAlphanumericString(4000);
            }

            HttpResponseMessage response = this.fixture.SendPostRequest(requestUrl, requirement);

            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [When(@"User creates following requirement without (contact|address form|country) using api")]
        public void UserCreatesFollowingRequirementWithoutAddressForm(string missingData, Table table)
        {
            string requestUrl = $"{ApiUrl}";

            var contacts = this.scenarioContext.Get<List<Contact>>("ContactList");
            var requirement = table.CreateInstance<CreateRequirementCommand>();
            var location = this.scenarioContext.Get<CreateOrUpdateRequirementAddress>("Location");

            requirement.CreateDate = DateTime.Now;

            if (!missingData.Equals("contact"))
            {
                requirement.ContactIds = contacts.Select(contact => contact.Id).ToList();
            }
            if (missingData.Equals("country"))
            {
                requirement.Address = new CreateOrUpdateRequirementAddress
                {
                    AddressFormId = location.AddressFormId,
                    CountryId = Guid.NewGuid(),
                    Line2 = location.Line2,
                    Postcode = location.Postcode,
                    City = location.City
                };
            }
            else if (missingData.Equals("address form"))
            {
                requirement.Address = new CreateOrUpdateRequirementAddress
                {
                    AddressFormId = Guid.NewGuid(),
                    CountryId = location.CountryId,
                    Line2 = location.Line2,
                    Postcode = location.Postcode,
                    City = location.City
                };
            }
            else
            {
                requirement.Address = location;
            }

            HttpResponseMessage response = this.fixture.SendPostRequest(requestUrl, requirement);

            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [When(@"User creates following requirement with invalid contact using api")]
        public void CreateRequirementWithInvalidContact(Table table)
        {
            string requestUrl = $"{ApiUrl}";

            var requirement = table.CreateInstance<CreateRequirementCommand>();

            requirement.CreateDate = DateTime.Now;
            requirement.ContactIds = new List<Guid> { Guid.NewGuid() };

            requirement.Address = this.scenarioContext.Get<CreateOrUpdateRequirementAddress>("Location");

            HttpResponseMessage response = this.fixture.SendPostRequest(requestUrl, requirement);

            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [When(@"User retrieves requirement for (.*) id")]
        public void WhenUserRetrievesRequirementForId(string id)
        {
            if (id.Equals("latest"))
            {
                id = this.scenarioContext.Get<Requirement>("Requirement").Id.ToString();
            }
            string requestUrl = $"{ApiUrl}/{id}";

            HttpResponseMessage response = this.fixture.SendGetRequest(requestUrl);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [Then(@"Requirement should be the same as added")]
        public void CompareRequirements()
        {
            var expectedRequirement = JsonConvert.DeserializeObject<Requirement>(this.scenarioContext.GetResponseContent());
            Requirement requirement = this.fixture.DataContext.Requirements.Single(req => req.Id.Equals(expectedRequirement.Id));

            AssertionOptions.AssertEquivalencyUsing(options =>
                options.Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation)).WhenTypeIs<DateTime>());

            requirement.ShouldBeEquivalentTo(expectedRequirement,
                opt => opt.Excluding(req => req.Address.Country).Excluding(req => req.Address.AddressForm).Excluding(req => req.RequirementNotes));
        }
    }
}
