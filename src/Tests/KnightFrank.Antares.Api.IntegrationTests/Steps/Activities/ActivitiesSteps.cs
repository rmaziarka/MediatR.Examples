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
    using KnightFrank.Antares.Domain.Activity.Commands;

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
                Contacts = new List<Contact>()
            };
            this.fixture.DataContext.Activities.Add(activity);
            this.fixture.DataContext.SaveChanges();

            this.scenarioContext.Set(activity, "Added Activity");
        }

        [Given(@"User creates activity for given (.*) property id")]
        [When(@"User creates activity for given (.*) property id")]
        public void WhenUserCreatesActivityWithActivityIdActivityStatusIdForPropertyId(string id)
        {
            string requestUrl = $"{ApiUrl}";

            Guid activityStatusId = this.scenarioContext.Get<Dictionary<string, Guid>>("EnumDictionary")["PreAppraisal"];
            Guid propertyId = id.Equals("latest") ? this.scenarioContext.Get<Guid>("AddedPropertyId") : new Guid(id);
            List<Guid> vendors =
                this.fixture.DataContext.Ownerships.Where(x => x.PropertyId.Equals(propertyId) && x.SellDate == null)
                    .SelectMany(x => x.Contacts).Select(c => c.Id)
                    .ToList();

            var activityCommand = new CreateActivityCommand
            {
                PropertyId = propertyId,
                ActivityStatusId = activityStatusId,
                ContactIds = vendors
            };

            HttpResponseMessage response = this.fixture.SendPostRequest(requestUrl, activityCommand);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [When(@"User retrieves activity details for given (.*)")]
        public void WhenUserRetrievesActivityDetailsForGiven(string activityId)
        {
            this.GetActivityResponse(activityId);
        }

        [When(@"User retrieves activity")]
        public void WhenUserRetrievesActivity()
        {
            var createdActivity = JsonConvert.DeserializeObject<Activity>(this.scenarioContext.GetResponseContent());
            this.GetActivityResponse(createdActivity.Id.ToString());
        }

        [When(@"User updates activity '(.*)' id and '(.*)' status with following sale valuation")]
        public void WhenUserUpdatesActivityWithFollowingSaleValuation(string id, string status, Table table)
        {
            string requestUrl = $"{ApiUrl}";

            var updateActivityCommand = table.CreateInstance<UpdateActivityCommand>();
            var activityFromDatabase = this.scenarioContext.Get<Activity>("Added Activity");
            updateActivityCommand.Id = id.Equals("added") ? activityFromDatabase.Id : new Guid(id);
            updateActivityCommand.ActivityStatusId = status.Equals("added") ? activityFromDatabase.ActivityStatusId 
                : new Guid(status);

            activityFromDatabase = new Activity
            {
                Id = updateActivityCommand.Id,
                ActivityStatusId = updateActivityCommand.ActivityStatusId,
                MarketAppraisalPrice = updateActivityCommand.MarketAppraisalPrice,
                RecommendedPrice = updateActivityCommand.RecommendedPrice,
                VendorEstimatedPrice = updateActivityCommand.VendorEstimatedPrice
            };

            this.scenarioContext["Added Activity"] = activityFromDatabase;

            HttpResponseMessage response = this.fixture.SendPutRequest(requestUrl, updateActivityCommand);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [Then(@"Activities list should be the same as in DB")]
        public void ThenActivitiesReturnedShouldBeTheSameAsInDatabase()
        {
            var propertyFromResponse = JsonConvert.DeserializeObject<Property>(this.scenarioContext.GetResponseContent());

            var activitiesFromDatabase = this.scenarioContext.Get<Activity>("Added Activity");

            AssertionOptions.AssertEquivalencyUsing(
                options => options.Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation)).WhenTypeIs<DateTime>());

            propertyFromResponse.Activities.ToList()[0].ShouldBeEquivalentTo(
                activitiesFromDatabase,
                options => options.Excluding(x => x.Property).Excluding(x => x.ActivityStatus));
        }

        [Then(@"The created Activity is saved in data base")]
        public void ThenTheCreatedActivityIsSavedInDataBase()
        {
            var activity = JsonConvert.DeserializeObject<Activity>(this.scenarioContext.GetResponseContent());
            Activity actualActivity = this.fixture.DataContext.Activities.Single(x => x.Id.Equals(activity.Id));

            actualActivity.ShouldBeEquivalentTo(
                activity,
                options => options.Excluding(x => x.Property).Excluding(x => x.ActivityStatus));

            actualActivity.ActivityStatus.Code.ShouldBeEquivalentTo("PreAppraisal");
        }

        [Then(@"The received Activities should be the same as in DB")]
        public void ThenTheReceivedActivitiesShouldBeTheSameAsInDataBase()
        {
            var expectedActivity = JsonConvert.DeserializeObject<Activity>(this.scenarioContext.GetResponseContent());
            Activity actualActivity = this.fixture.DataContext.Activities.Single(x => x.Id.Equals(expectedActivity.Id));

            actualActivity.ShouldBeEquivalentTo(
                expectedActivity,
                options => options.Excluding(a => a.ActivityStatus).Excluding(a => a.Contacts).Excluding(a => a.Property));
        }

        private void GetActivityResponse(string activityId)
        {
            string requestUrl = $"{ApiUrl}/{activityId}";

            HttpResponseMessage response = this.fixture.SendGetRequest(requestUrl);
            this.scenarioContext.SetHttpResponseMessage(response);
        }
    }
}
