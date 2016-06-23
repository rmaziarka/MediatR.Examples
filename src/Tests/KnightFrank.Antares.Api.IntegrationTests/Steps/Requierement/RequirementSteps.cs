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
    using KnightFrank.Antares.Domain.Common.Commands;
    using KnightFrank.Antares.Domain.Common.Enums;
    using KnightFrank.Antares.Domain.Requirement.Commands;

    using Newtonsoft.Json;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    using Xunit;

    [Binding]
    public class RequirementSteps : IClassFixture<BaseTestClassFixture>
    {
        private const string ApiUrl = "/api/requirements";
        private readonly DateTime date = DateTime.UtcNow;
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

        [Given(@"Requirement exists in database")]
        public void RequirementExistsInDatabase()
        {
            const string countryCode = "GB";
            Guid countryId = this.fixture.DataContext.Countries.Single(x => x.IsoCode.Equals(countryCode)).Id;
            Guid enumTypeItemId =
                this.fixture.DataContext.EnumTypeItems.Single(
                    e => e.EnumType.Code.Equals(nameof(EntityType)) && e.Code.Equals(nameof(EntityType.Requirement))).Id;
            Guid addressFormId =
                this.fixture.DataContext.AddressFormEntityTypes.Single(
                    afe => afe.AddressForm.CountryId == countryId && afe.EnumTypeItemId == enumTypeItemId).AddressFormId;

            var contacts = this.scenarioContext.Get<List<Contact>>("Contacts");

            var address = new Address
            {
                AddressFormId = addressFormId,
                CountryId = countryId,
                City = StringExtension.GenerateMaxAlphanumericString(128),
                Line2 = StringExtension.GenerateMaxAlphanumericString(128),
                Postcode = StringExtension.GenerateMaxAlphanumericString(10)
            };

            var requirement = new Requirement
            {
                CreateDate = this.date,
                Contacts = contacts,
                Address = address,
                MinArea = 1,
                MaxArea = 2,
                MinBathrooms = 1,
                MaxBathrooms = 2,
                MinBedrooms = 1,
                MaxBedrooms = 2,
                MinLandArea = 1,
                MaxLandArea = 2,
                MinPrice = 1,
                MaxPrice = 2,
                MinParkingSpaces = 1,
                MaxParkingSpaces = 2,
                MinReceptionRooms = 1,
                MaxReceptionRooms = 2,
                Description = StringExtension.GenerateMaxAlphanumericString(4000)
            };

            this.fixture.DataContext.Requirements.Add(requirement);
            this.fixture.DataContext.SaveChanges();

            this.scenarioContext.Set(requirement, "Requirement");
        }

        [When(@"User sets locations details for the requirement")]
        public void SetRequirementLocationDetails(Table table)
        {
            var details = table.CreateInstance<CreateOrUpdateAddress>();
            details.AddressFormId = this.scenarioContext.Get<Guid>("AddressFormId");
            details.CountryId = this.scenarioContext.Get<Guid>("CountryId");

            this.scenarioContext.Set(details, "Location");
        }

        [When(@"User sets locations details for the requirement with max length fields")]
        public void SetRequirementLocationDetailsWithMaxFields()
        {
            var details = new CreateOrUpdateAddress
            {
                City = StringExtension.GenerateMaxAlphanumericString(128),
                Line2 = StringExtension.GenerateMaxAlphanumericString(128),
                Postcode = StringExtension.GenerateMaxAlphanumericString(10),
                AddressFormId = this.scenarioContext.Get<Guid>("AddressFormId"),
                CountryId = this.scenarioContext.Get<Guid>("CountryId")
            };

            this.scenarioContext.Set(details, "Location");
        }

        [When(@"User creates following requirement using api")]
        public void CreateRequirementWithApi(Table table)
        {
            string requestUrl = $"{ApiUrl}";

            var contacts = this.scenarioContext.Get<List<Contact>>("Contacts");
            var requirement = table.CreateInstance<CreateRequirementCommand>();

            requirement.CreateDate = DateTime.Now;
            requirement.ContactIds = contacts.Select(contact => contact.Id).ToList();

            requirement.Address = this.scenarioContext.Get<CreateOrUpdateAddress>("Location");
            if (requirement.Description.ToLower().Equals("max"))
            {
                requirement.Description = StringExtension.GenerateMaxAlphanumericString(4000);
            }

            HttpResponseMessage response = this.fixture.SendPostRequest(requestUrl, requirement);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [When(@"User creates requirement with mandatory fields using api")]
        public void CreateRequirementWithMandatoryFieldsWithApi()
        {
            string requestUrl = $"{ApiUrl}";

            var contacts = this.scenarioContext.Get<List<Contact>>("Contacts");

            var requirement = new CreateRequirementCommand
            {
                ContactIds = contacts.Select(contact => contact.Id).ToList(),
                Address = new CreateOrUpdateAddress
                {
                    City = string.Empty,
                    Line2 = string.Empty,
                    Postcode = string.Empty,
                    AddressFormId = this.scenarioContext.Get<Guid>("AddressFormId"),
                    CountryId = this.scenarioContext.Get<Guid>("CountryId")
                }
            };

            HttpResponseMessage response = this.fixture.SendPostRequest(requestUrl, requirement);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [When(@"User creates following requirement without (contact|address form|country) using api")]
        public void UserCreatesFollowingRequirementWithoutAddressForm(string missingData, Table table)
        {
            string requestUrl = $"{ApiUrl}";

            var contacts = this.scenarioContext.Get<List<Contact>>("Contacts");
            var requirement = table.CreateInstance<CreateRequirementCommand>();
            var location = this.scenarioContext.Get<CreateOrUpdateAddress>("Location");

            requirement.CreateDate = DateTime.Now;

            if (!missingData.Equals("contact"))
            {
                requirement.ContactIds = contacts.Select(contact => contact.Id).ToList();
            }
            if (missingData.Equals("country"))
            {
                requirement.Address = new CreateOrUpdateAddress
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
                requirement.Address = new CreateOrUpdateAddress
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

            requirement.Address = this.scenarioContext.Get<CreateOrUpdateAddress>("Location");

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
            var currentRequirement = JsonConvert.DeserializeObject<Requirement>(this.scenarioContext.GetResponseContent());
            Requirement expectedRequirement =
                this.fixture.DataContext.Requirements.Single(req => req.Id.Equals(currentRequirement.Id));

            AssertionOptions.AssertEquivalencyUsing(options =>
                options.Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation)).WhenTypeIs<DateTime>());

            expectedRequirement.ShouldBeEquivalentTo(currentRequirement, opt => opt.
                Excluding(req => req.Address.Country).
                Excluding(req => req.Address.AddressForm));
        }
    }
}
