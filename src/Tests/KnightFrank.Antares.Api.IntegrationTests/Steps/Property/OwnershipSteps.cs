namespace KnightFrank.Antares.Api.IntegrationTests.Steps.Property
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

        [Given(@"Ownership exists in database")]
        public void GivenFollowingOwnershipExistsInDataBase(Table table)
        {
            List<Ownership> ownerships = table.CreateSet<Ownership>().ToList();
            foreach (Ownership ownership in ownerships)
            {
                ownership.PropertyId = this.scenarioContext.Get<Guid>("AddedPropertyId");
                ownership.OwnershipTypeId = this.scenarioContext.Get<Dictionary<string, Guid>>("EnumDictionary")["Freeholder"];
                ownership.Contacts = this.scenarioContext.Get<ICollection<Contact>>("Contact List");
            }

            this.fixture.DataContext.Ownerships.AddRange(ownerships);
            this.fixture.DataContext.SaveChanges();

            this.scenarioContext.Set(ownerships, "Added Ownership List");
        }

        [When(@"User creates an ownership for existing property")]
        public void WhenUserCreatesAnOwnershipForExistingProperty(Table table)
        {
            var ownership = table.CreateInstance<CreateOwnershipCommand>();
            var propertyId = this.scenarioContext.Get<Guid>("AddedPropertyId");

            string requestUrl = string.Format($"{ApiUrl}", propertyId);
            ownership.OwnershipTypeId = this.scenarioContext.Get<Dictionary<string, Guid>>("EnumDictionary")["Freeholder"];
            ownership.ContactIds = this.scenarioContext.Get<ICollection<Contact>>("Contact List").Select(x => x.Id).ToList();

            HttpResponseMessage response = this.fixture.SendPostRequest(requestUrl, ownership);
            this.scenarioContext.SetHttpResponseMessage(response);

            this.scenarioContext.Set(ownership, "Added Ownership");
        }

        [Then(@"Ownership list should be the same as in database")]
        public void ThenOwnershipReturnedShouldBeTheSameAsInDatabase()
        {
            var propertyFromResponse = JsonConvert.DeserializeObject<Property>(this.scenarioContext.GetResponseContent());

            ICollection<Ownership> ownershipFromDatabase =
                this.fixture.DataContext.Properties.Single(prop => prop.Id.Equals(propertyFromResponse.Id)).Ownerships;

            propertyFromResponse.Ownerships.ShouldAllBeEquivalentTo(ownershipFromDatabase, options => options
                .Excluding(x => x.Property)
                .Excluding(x => x.OwnershipType));
        }

        [Then(@"Created Ownership is saved in database")]
        public void ThenTheResultsShouldBeSameAsCreated()
        {
            var propertyId = this.scenarioContext.Get<Guid>("AddedPropertyId");
            var actualOwnership = this.scenarioContext.Get<CreateOwnershipCommand>("Added Ownership");

            Ownership expectedOwnership = this.fixture.DataContext.Ownerships.Single(x => x.PropertyId.Equals(propertyId));

            actualOwnership.ShouldBeEquivalentTo(expectedOwnership, options => options
                .Excluding(x => x.ContactIds)
                .Excluding(x => x.PropertyId));

            propertyId.ShouldBeEquivalentTo(expectedOwnership.PropertyId);
            actualOwnership.ContactIds.ShouldAllBeEquivalentTo(expectedOwnership.Contacts.Select(x => x.Id));
        }

        [Then(@"Response contains property with ownership")]
        public void ThenResponseContainsPropertyWithOwnership()
        {
            var propertyId = this.scenarioContext.Get<Guid>("AddedPropertyId");

            var actualOwnership = JsonConvert.DeserializeObject<Ownership>(this.scenarioContext.GetResponseContent());

            Ownership expectedOwnership =
                this.fixture.DataContext.Ownerships.Single(x => x.PropertyId.Equals(propertyId));

            actualOwnership.ShouldBeEquivalentTo(expectedOwnership, options => options
                .Excluding(x => x.Property)
                .Excluding(x => x.OwnershipType));
        }
    }
}
