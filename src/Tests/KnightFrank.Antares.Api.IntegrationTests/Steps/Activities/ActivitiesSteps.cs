namespace KnightFrank.Antares.Api.IntegrationTests.Steps.Activities
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
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.Activity.QueryResults;

    using Newtonsoft.Json;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class ActivitiesSteps
    {
        private const string ApiUrl = "/api/activities";
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

        [Given(@"All activities have been deleted from database")]
        public void DeleteActivities()
        {
            this.fixture.DataContext.Activities.RemoveRange(this.fixture.DataContext.Activities.ToList());
        }

        [Given(@"Activity for (.*) property and (.*) activity status exists in database")]
        public void CreateActivityInDatabase(string id, string activityStatus)
        {
            Guid activityStatusId = this.scenarioContext.Get<Dictionary<string, Guid>>("EnumDictionary")[activityStatus];
            Guid propertyId = id.Equals("latest") ? this.scenarioContext.Get<Guid>("AddedPropertyId") : new Guid(id);
            var activityTypeId = this.scenarioContext.Get<Guid>("ActivityTypeId");

            var activity = new Activity
            {
                PropertyId = propertyId,
                ActivityTypeId = activityTypeId,
                ActivityStatusId = activityStatusId,
                CreatedDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
                Contacts = new List<Contact>()
            };

            this.fixture.DataContext.Activities.Add(activity);
            this.fixture.DataContext.SaveChanges();

            this.scenarioContext.Set(activity, "Activity");
        }

        [Given(@"User creates activity for given (.*) property id")]
        [When(@"User creates activity for given (.*) property id")]
        public void CreateActivityUsingApi(string id)
        {
            string requestUrl = $"{ApiUrl}";

            Guid activityStatusId = this.scenarioContext.Get<Dictionary<string, Guid>>("EnumDictionary")["PreAppraisal"];
            Guid propertyId = id.Equals("latest") ? this.scenarioContext.Get<Guid>("AddedPropertyId") : new Guid(id);
            var activityTypeId = this.scenarioContext.Get<Guid>("ActivityTypeId");

            List<Guid> vendors =
                this.fixture.DataContext.Ownerships.Where(x => x.PropertyId.Equals(propertyId) && x.SellDate == null)
                    .SelectMany(x => x.Contacts).Select(c => c.Id)
                    .ToList();

            var activityCommand = new CreateActivityCommand
            {
                PropertyId = propertyId,
                ActivityStatusId = activityStatusId,
                ContactIds = vendors,
                ActivityTypeId = activityTypeId
            };

            HttpResponseMessage response = this.fixture.SendPostRequest(requestUrl, activityCommand);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [Given(@"User gets (.*) for ActivityType")]
        public void GetActivityTypeId(string activityTypeCode)
        {
            if (activityTypeCode.Equals("invalid"))
            {
                this.scenarioContext.Set(Guid.NewGuid(), "ActivityTypeId");
            }
            else
            {
                Guid activityTypeId = this.fixture.DataContext.ActivityTypes.Single(i => i.Code.Equals(activityTypeCode)).Id;
                this.scenarioContext.Set(activityTypeId, "ActivityTypeId");
            }
        }

        [When(@"User updates activity (.*) id and (.*) status with following sale valuation")]
        public void WhenUserUpdatesActivityWithFollowingSaleValuation(string id, string status, Table table)
        {
            string requestUrl = $"{ApiUrl}";

            var updateActivityCommand = table.CreateInstance<UpdateActivityCommand>();
            var activityFromDatabase = this.scenarioContext.Get<Activity>("Activity");

            updateActivityCommand.Id = id.Equals("latest") ? activityFromDatabase.Id : new Guid(id);
            updateActivityCommand.ActivityTypeId = activityFromDatabase.ActivityTypeId;
            updateActivityCommand.ActivityStatusId = status.Equals("latest")
                ? activityFromDatabase.ActivityStatusId
                : new Guid(status);

            activityFromDatabase = new Activity
            {
                Id = updateActivityCommand.Id,
                ActivityStatusId = updateActivityCommand.ActivityStatusId,
                ActivityTypeId = updateActivityCommand.ActivityTypeId,
                MarketAppraisalPrice = updateActivityCommand.MarketAppraisalPrice,
                RecommendedPrice = updateActivityCommand.RecommendedPrice,
                VendorEstimatedPrice = updateActivityCommand.VendorEstimatedPrice
            };

            this.scenarioContext["Activity"] = activityFromDatabase;

            HttpResponseMessage response = this.fixture.SendPutRequest(requestUrl, updateActivityCommand);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [When(@"User gets activity with (.*) id")]
        public void WhenUserRetrievesActivityDetailsForGiven(string activityId)
        {
            this.GetActivityResponse(activityId);
        }

        [When(@"User gets activity")]
        public void WhenUserRetrievesActivity()
        {
            var createdActivity = JsonConvert.DeserializeObject<Activity>(this.scenarioContext.GetResponseContent());
            this.GetActivityResponse(createdActivity.Id.ToString());
        }

        [When(@"User gets activities")]
        public void GetAllActivities()
        {
            this.GetActivityResponse();
        }

        [Then(@"Activities list should be the same as in database")]
        public void ThenActivitiesReturnedShouldBeTheSameAsInDatabase()
        {
            var propertyFromResponse = JsonConvert.DeserializeObject<Property>(this.scenarioContext.GetResponseContent());

            Activity activitiesFromDatabase =
                this.fixture.DataContext.Properties.Single(prop => prop.Id.Equals(propertyFromResponse.Id)).Activities.First();

            AssertionOptions.AssertEquivalencyUsing(
                options => options.Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation)).WhenTypeIs<DateTime>());

            propertyFromResponse.Activities.ToList()[0].ShouldBeEquivalentTo(activitiesFromDatabase, options => options
                .Excluding(x => x.Property)
                .Excluding(x => x.ActivityStatus)
                .Excluding(x => x.ActivityType));
        }

        [Then(@"Created Activity is saved in database")]
        public void ThenTheCreatedActivityIsSavedInDataBase()
        {
            var activity = JsonConvert.DeserializeObject<Activity>(this.scenarioContext.GetResponseContent());
            Activity actualActivity = this.fixture.DataContext.Activities.Single(x => x.Id.Equals(activity.Id));

            actualActivity.ShouldBeEquivalentTo(activity, options => options
                .Excluding(x => x.Property)
                .Excluding(x => x.ActivityStatus)
                .Excluding(x => x.ActivityType));

            actualActivity.ActivityStatus.Code.ShouldBeEquivalentTo("PreAppraisal");
        }

        [Then(@"Retrieved activity should be same as in database")]
        public void ThenTheReceivedActivitiesShouldBeTheSameAsInDataBase()
        {
            var activity = JsonConvert.DeserializeObject<Activity>(this.scenarioContext.GetResponseContent());
            Activity actualActivity = this.fixture.DataContext.Activities.Single(x => x.Id.Equals(activity.Id));

            actualActivity.ShouldBeEquivalentTo(activity, options => options
                .Excluding(a => a.ActivityStatus)
                .Excluding(a => a.Contacts)
                .Excluding(a => a.Property)
                .Excluding(a => a.ActivityType));
        }

        [Then(@"Retrieved activities should be the same as in database")]
        public void CheckActivities(Table table)
        {
            var activity = JsonConvert.DeserializeObject<List<ActivitiesQueryResult>>(this.scenarioContext.GetResponseContent());
            List<ActivitiesQueryResult> actualActivity = table.CreateSet<ActivitiesQueryResult>().ToList();
            actualActivity.First().ActivityId = this.scenarioContext.Get<Activity>("Activity").Id;

            actualActivity.ShouldBeEquivalentTo(activity);
        }

        private void GetActivityResponse(string activityId)
        {
            string requestUrl = $"{ApiUrl}/{activityId}";

            HttpResponseMessage response = this.fixture.SendGetRequest(requestUrl);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        private void GetActivityResponse()
        {
            string requestUrl = $"{ApiUrl}";

            HttpResponseMessage response = this.fixture.SendGetRequest(requestUrl);
            this.scenarioContext.SetHttpResponseMessage(response);
        }
    }
}
