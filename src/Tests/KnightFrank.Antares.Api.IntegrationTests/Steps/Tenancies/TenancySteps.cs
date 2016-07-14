namespace KnightFrank.Antares.Api.IntegrationTests.Steps.Tenancies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    using FluentAssertions;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
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
                    StartDate = new DateTime(2016, 2, 2),
                    EndDate = new DateTime(2017, 2, 2)
                }
            };

            this.PostTenancy(tenancyCommand);
        }

        [Then(@"Tenancy should be the same as added")]
        public void ThenTenancyShouldBeTheSameAsAdded()
        {
            var currectTenancy = JsonConvert.DeserializeObject<Tenancy>(this.scenarioContext.GetResponseContent());
            Tenancy expectedTenancy =
                this.fixture.DataContext.Tenancies.Single(req => req.Id.Equals(currectTenancy.Id));

            expectedTenancy.ActivityId.Should().Be(currectTenancy.ActivityId);
            expectedTenancy.RequirementId.Should().Be(currectTenancy.RequirementId);
            expectedTenancy.Terms.First().ShouldBeEquivalentTo(currectTenancy.Terms.First());
        }

        private void PostTenancy(CreateTenancyCommand command)
        {
            HttpResponseMessage response = this.fixture.SendPostRequest(ApiUrl, command);
            this.scenarioContext.SetHttpResponseMessage(response);
        }
    }
}
