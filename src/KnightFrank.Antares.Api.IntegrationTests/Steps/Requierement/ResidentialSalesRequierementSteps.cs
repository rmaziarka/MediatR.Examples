﻿namespace KnightFrank.Antares.Api.IntegrationTests.Steps.Requierement
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
    using KnightFrank.Antares.Domain.Contact;
    using KnightFrank.Antares.Domain.Requirement.Commands;

    using Newtonsoft.Json;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    using Xunit;

    [Binding]
    public class ResidentialSalesRequierementSteps : IClassFixture<BaseTestClassFixture>
    {
        private const string ApiUrl = "/api/requirements";
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

        [When(@"User sets locations details for the requirement")]
        public void SetRequirementLocationDetails(Table table)
        {
            var details = table.CreateInstance<CreateOrUpdateRequirementAddress>();
            details.AddressFormId = this.scenarioContext.Get<Guid>("AddressFormId");
            details.CountryId = this.scenarioContext.Get<Guid>("CountryId");
            this.scenarioContext.Set(details, "Location");
        }

        [When(@"User creates following requirement in database")]
        public void CreateRequirementWithInDb(Table table)
        {
            var requirement = table.CreateInstance<Requirement>();

            requirement.CreateDate = DateTime.Now;
            requirement.Contacts.AddRange(this.scenarioContext.Get<List<Contact>>("Contact List"));

            var location = this.scenarioContext.Get<CreateOrUpdateRequirementAddress>("Location");
            requirement.Address = new Address
            {
                Line2 = location.Line2,
                Postcode = location.Postcode,
                City = location.City,
                CountryId = location.CountryId,
                AddressFormId = location.AddressFormId
            };

            this.fixture.DataContext.Requirement.Add(requirement);
            this.fixture.DataContext.SaveChanges();

            this.scenarioContext.Set(requirement, "Requirement");
        }

        [When(@"User creates following requirement using api")]
        public void CreateRequirementWithApi(Table table)
        {
            string requestUrl = $"{ApiUrl}";

            var contacts = this.scenarioContext.Get<List<Contact>>("Contact List");
            var requirement = table.CreateInstance<CreateRequirementCommand>();

            requirement.CreateDate = DateTime.Now;
            requirement.Contacts = contacts.Select(contact => new ContactDto
            {
                Id = contact.Id,
                FirstName = contact.FirstName,
                Surname = contact.Surname,
                Title = contact.Title
            }).ToList();

            requirement.Address = this.scenarioContext.Get<CreateOrUpdateRequirementAddress>("Location");

            HttpResponseMessage response = this.fixture.SendPostRequest(requestUrl, requirement);

            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [When(@"User creates following requirement without (contact|address form|country) using api")]
        public void UserCreatesFollowingRequirementWithoutAddressForm(string missingData, Table table)
        {
            string requestUrl = $"{ApiUrl}";

            var contacts = this.scenarioContext.Get<List<Contact>>("Contact List");
            var requirement = table.CreateInstance<CreateRequirementCommand>();
            var location = this.scenarioContext.Get<CreateOrUpdateRequirementAddress>("Location");

            requirement.CreateDate = DateTime.Now;

            if (!missingData.Equals("contact"))
            {
                requirement.Contacts = contacts.Select(contact => new ContactDto
                {
                    Id = contact.Id,
                    FirstName = contact.FirstName,
                    Surname = contact.Surname,
                    Title = contact.Title
                }).ToList();
            }
            if (missingData.Equals("country"))
            {
                requirement.Address = new CreateOrUpdateRequirementAddress
                {
                    AddressFormId = location.AddressFormId,
                    CountryId = Guid.Empty,
                    Line2 = location.Line2,
                    Postcode = location.Postcode,
                    City = location.City
                };
            }
            else if (missingData.Equals("address form"))
            {
                requirement.Address = new CreateOrUpdateRequirementAddress
                {
                    AddressFormId = Guid.Empty,
                    CountryId = location.CountryId,
                    Line2 = location.Line2,
                    Postcode = location.Postcode,
                    City = location.City
                };
            }
            else
                requirement.Address = location;

            HttpResponseMessage response = this.fixture.SendPostRequest(requestUrl, requirement);

            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [When(@"User creates following requirement with invalid contact using api")]
        public void CreateRequirementWithInvalidContact(Table table)
        {
            string requestUrl = $"{ApiUrl}";

            var requirement = table.CreateInstance<CreateRequirementCommand>();

            requirement.CreateDate = DateTime.Now;
            requirement.Contacts = new List<ContactDto>
            {
                new ContactDto { Id = Guid.Empty }
            };

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
            string requestUrl = $"{ApiUrl}/" + id + "";

            HttpResponseMessage response = this.fixture.SendGetRequest(requestUrl);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [Then(@"Requirement should be the same as added")]
        public void CompareRequirements()
        {
            var expectedRequirement = JsonConvert.DeserializeObject<Requirement>(this.scenarioContext.GetResponseContent());
            Requirement requirement = this.fixture.DataContext.Requirement.Single(req => req.Id.Equals(expectedRequirement.Id));

            AssertionOptions.AssertEquivalencyUsing(options =>
                options.Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation)).WhenTypeIs<DateTime>()
                );

            requirement.ShouldBeEquivalentTo(expectedRequirement, opt => opt.Excluding(req => req.Address.Country).Excluding(req => req.Address.AddressForm));
        }
    }
}
