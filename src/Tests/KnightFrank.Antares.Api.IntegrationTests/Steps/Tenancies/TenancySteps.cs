namespace KnightFrank.Antares.Api.IntegrationTests.Steps.Tenancies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    using FluentAssertions;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Model.Tenancy;
    using KnightFrank.Antares.Domain.Tenancy.Commands;

    using Newtonsoft.Json;

    using TechTalk.SpecFlow;

    using Xunit;

    [Binding]
    public class TenancySteps : IClassFixture<BaseTestClassFixture>
    {
        private const string ApiUrl = "/api/tenancies";
        private readonly BaseTestClassFixture fixture;
        private readonly ScenarioContext scenarioContext;
        private readonly DateTime date = DateTime.UtcNow;

        public TenancySteps(BaseTestClassFixture fixture, ScenarioContext scenarioContext)
        {
            this.fixture = fixture;
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            this.scenarioContext = scenarioContext;
        }

        [When(@"User creates tenancy for latest requirement and activity")]
        public void WhenUserCreatesTenancyForLatestRequirementAndActivity()
        {
            Guid requirementId = this.scenarioContext.Get<Requirement>("Requirement").Id;
            Guid activityId = this.scenarioContext.Get<Activity>("Activity").Id;

            var tenancyCommand = new CreateTenancyCommand
            {
                ActivityId = activityId,
                RequirementId = requirementId,
                LandlordContacts = new List<Guid>(),
                TenantContacts = new List<Guid>(),
                Term = new UpdateTenancyTerm
                {
                    AgreedRent = 200,
                    StartDate = this.date.AddDays(1),
                    EndDate = this.date.AddYears(1)
                }
            };

            this.PostTenancy(tenancyCommand);
        }

        [When(@"User creates tenancy with invalid (.*) using api")]
        public void WhenUserCreatesTenancyWithInvalidDataUsingApi(string data)
        {
            Guid requirementId = data.Equals("requirement") ? Guid.NewGuid() : this.scenarioContext.Get<Requirement>("Requirement").Id;
            Guid activityId = data.Equals("activity") ? Guid.NewGuid() : this.scenarioContext.Get<Activity>("Activity").Id;

            var tenancyCommand = new CreateTenancyCommand
            {
                ActivityId = activityId,
                RequirementId = requirementId,
                LandlordContacts = new List<Guid>(),
                TenantContacts = new List<Guid>(),
                Term = new UpdateTenancyTerm
                {
                    AgreedRent = 200,
                    StartDate = this.date.AddDays(1),
                    EndDate = this.date.AddYears(1)
                }
            };

            this.PostTenancy(tenancyCommand);
        }


        [Then(@"Tenancy should be the same as added")]
        public void ThenTenancyShouldBeTheSameAsAdded()
        {
            var currentTenancy = JsonConvert.DeserializeObject<Tenancy>(this.scenarioContext.GetResponseContent());
            Tenancy expectedTenancy =
                this.fixture.DataContext.Tenancies.Single(req => req.Id.Equals(currentTenancy.Id));

            expectedTenancy.ActivityId.Should().Be(currentTenancy.ActivityId);
            expectedTenancy.RequirementId.Should().Be(currentTenancy.RequirementId);
            expectedTenancy.Terms.First().ShouldBeEquivalentTo(currentTenancy.Terms.First());
        }

        private void PostTenancy(CreateTenancyCommand command)
        {
            HttpResponseMessage response = this.fixture.SendPostRequest(ApiUrl, command);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        private void UpdateTenancy(UpdateTenancyCommand command)
        {
            HttpResponseMessage response = this.fixture.SendPutRequest(ApiUrl, command);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [Given(@"tenancy exists in database")]
        public void GivenTenancyExistsInDatabase()
        {
            var requirementId = this.scenarioContext.Get<Requirement>("Requirement").Id;
            var activityId = this.scenarioContext.Get<Activity>("Activity").Id;
            var tenancy = new Tenancy
            {
                RequirementId = requirementId,
                ActivityId = activityId,
                TenancyTypeId = this.fixture.DataContext.TenancyTypes.Single(t => t.EnumCode == nameof(Domain.Common.Enums.TenancyType.ResidentialLetting)).Id
            };

            this.fixture.DataContext.Tenancies.Add(tenancy);
            this.fixture.DataContext.SaveChanges();

            this.fixture.DataContext.Terms.Add(new TenancyTerm { AgreedRent = 10, EndDate = this.date.AddYears(1) , StartDate = this.date.AddDays(1) , TenancyId = tenancy.Id });
            this.fixture.DataContext.SaveChanges();

            this.scenarioContext.Set(tenancy, "Tenancy");
        }

        [When(@"User updates tenancy with terms")]
        public void WhenUserUpdatesTenancyWithTerms()
        {
            Guid tenancyId = this.scenarioContext.Get<Tenancy>("Tenancy").Id;

            var upadteTenancyCommand = new UpdateTenancyCommand
            {
                TenancyId = tenancyId,
                Term = new UpdateTenancyTerm
                {
                    AgreedRent = 200,
                    StartDate = this.date.AddDays(1),
                    EndDate = this.date.AddYears(1)
                }
            };

            this.UpdateTenancy(upadteTenancyCommand);
        }

        [When(@"User gets tenancy for (.*) id")]
        public void GetTenancy(string id)
        {
            Guid tenancyId = id.Equals("latest") ? this.scenarioContext.Get<Tenancy>("Tenancy").Id : Guid.NewGuid();
            string requestUrl = $"{ApiUrl}/{tenancyId}";

            HttpResponseMessage response = this.fixture.SendGetRequest(requestUrl);
            this.scenarioContext.SetHttpResponseMessage(response);
        }
    }
}
