namespace KnightFrank.Antares.Api.IntegrationTests.Steps.Ownership
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    using FluentAssertions;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Dal.Model.Contact;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Domain.Ownership.Commands;

    using Newtonsoft.Json;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class OwnershipSteps
    {
        private const string ApiUrl = "/api/properties/{0}/ownerships";
        private readonly BaseTestClassFixture fixture;

        private readonly ScenarioContext scenarioContext;

        public OwnershipSteps(BaseTestClassFixture fixture, ScenarioContext scenarioContext)
        {
            this.fixture = fixture;
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            this.scenarioContext = scenarioContext;
        }

        [When(@"User creates an ownership for existing property")]
        public void WhenUserCreatesAnOwnershipForExistingProperty(Table table)
        {
            var ownership = table.CreateInstance<CreateOwnershipCommand>();

            string requestUrl = string.Format($"{ApiUrl}", ownership.PropertyId);

            ownership.PropertyId = this.scenarioContext.Get<Guid>("AddedPropertyId");
            ownership.OwnershipTypeId = this.scenarioContext.Get<Guid>("EnumTypeItemId");
            ownership.ContactIds = this.scenarioContext.Get<ICollection<Contact>>("Contact List").Select(x => x.Id).ToList();

            HttpResponseMessage response = this.fixture.SendPostRequest(requestUrl, ownership);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [Then(@"Ownership added should be the same as sent in request")]
        public void ThenOwnershipShouldBeTheSameAsSentInRequest()
        {
            var tableRequirement = this.scenarioContext.Get<CreateOwnershipCommand>("Ownership");

            AssertionOptions.AssertEquivalencyUsing(options =>
                options.Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation)).WhenTypeIs<DateTime>()
                );

            var databaseRequirement = JsonConvert.DeserializeObject<Ownership>(this.scenarioContext.GetResponseContent());
            databaseRequirement.ShouldBeEquivalentTo(tableRequirement);
        }
    }
}
