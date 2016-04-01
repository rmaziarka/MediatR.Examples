namespace KnightFrank.Antares.Api.IntegrationTests.Steps.Activities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    using FluentAssertions;
    using FluentAssertions.Common;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Domain.Activity.Commands;

    using Newtonsoft.Json;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class ActivitiesSteps
    {
        private const string ApiUrl = "/api/Activities";
        private readonly BaseTestClassFixture fixture;
        private readonly ScenarioContext scenarioContext;

        public ActivitiesSteps(BaseTestClassFixture fixture, ScenarioContext scenarioContext)
        {
            this.fixture = fixture;
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            this.scenarioContext = scenarioContext;
        }

        [Given(@"There is activity type id and activity status id defined")]
        public void GivenThereIsActivityTypeIdAndActivityStatusIdDefined(Guid id)
        {
            IQueryable<Contact> abc =
                this.fixture.DataContext.Ownerships.Where(x => x.PropertyId == id && x.SellDate == null)
                    .SelectMany(x => x.Contacts);
        }

        [Given(@"Activity for '(.*)' property exists in data base")]
        public void GivenActivityForPropertyExistsInDataBase(string id)
        {
            Guid activityStatusId = this.scenarioContext.Get<Dictionary<string, Guid>>("EnumDictionary")["PreAppraisal"];
            Guid propertyId = id.Equals("latest") ? this.scenarioContext.Get<Guid>("AddedPropertyId") : new Guid(id);

            var activity = new Activity
            {
                PropertyId = propertyId,
                ActivityStatusId = activityStatusId,
                CreatedDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
                ActivityTypeId = activityStatusId,
                Contacts = new List<Contact>()
               
               // Property = this.fixture.DataContext.Property.SingleOrDefault(x => x.Id == propertyId)
            };
            this.fixture.DataContext.Activity.Add(activity);
            this.fixture.DataContext.SaveChanges();

            this.scenarioContext.Set<Activity>(activity, "Added Activity");
        }

        [When(@"User creates activity for given (.*) property id")]
        public void WhenUserCreatesActivityWithActivityIdActivityStatusIdForPropertyId(string id)
        {
            string requestUrl = $"{ApiUrl}";

            Guid activityStatusId = this.scenarioContext.Get<Dictionary<string, Guid>>("EnumDictionary")["PreAppraisal"];
            Guid propertyId = id.Equals("latest") ? this.scenarioContext.Get<Guid>("AddedPropertyId") : new Guid(id);

            var activityCommand = new CreateActivityCommand { PropertyId = propertyId, ActivityStatusId = activityStatusId };

            HttpResponseMessage response = this.fixture.SendPostRequest(requestUrl, activityCommand);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [Then(@"Activities list should be the same as in DB")]
        public void ThenActivitiesReturnedShouldBeTheSameAsInDatabase()
        {
            var propertyFromResponse = JsonConvert.DeserializeObject<Property>(this.scenarioContext.GetResponseContent());

            var activitiesFromDatabase = this.scenarioContext.Get<Activity>("Added Activity");

            AssertionOptions.AssertEquivalencyUsing(options =>
                options.Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation)).WhenTypeIs<DateTime>());

            propertyFromResponse.Activities.ToList()[0].ShouldBeEquivalentTo(activitiesFromDatabase, options => options
                .Excluding(x => x.Property)
                .Excluding(x => x.ActivityStatus)
                .Excluding(x => x.ActivityType));
        }
    }
}
