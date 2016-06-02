namespace KnightFrank.Antares.Api.IntegrationTests.Steps.Activities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    using FluentAssertions;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Api.IntegrationTests.Steps.Enums;
    using KnightFrank.Antares.Api.IntegrationTests.Steps.Property;
    using KnightFrank.Antares.Dal.Model.Attachment;
    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Offer;
    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.Activity.QueryResults;
    using KnightFrank.Antares.Domain.Common.Enums;

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

        [Given(@"Activity exists in database")]
        public void GivenActivityExistsInDb()
        {
            var enumTable = new Table("enumTypeCode", "enumTypeItemCode");
            enumTable.AddRow("ActivityStatus", "PreAppraisal");
            enumTable.AddRow("Division", "Residential");
            enumTable.AddRow("ActivityDocumentType", "TermsOfBusiness");
            enumTable.AddRow("ActivityUserType", "LeadNegotiator");

            var addressTable = new Table("Postcode");
            addressTable.AddRow("N1C");

            var properstySteps = new PropertySteps(this.fixture, this.scenarioContext);
            properstySteps.GetCountryAddressData("GB", "Property");
            properstySteps.GetPropertyTypeId("House");
            this.GetActivityTypeId("Freehold Sale");

            new EnumsSteps(this.fixture, this.scenarioContext).GetEnumTypeItemId(enumTable);

            properstySteps.GivenFollowingPropertyExistsInDataBase("Residential", addressTable);
            this.CreateActivityInDatabase("latest", "PreAppraisal");
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

            Guid userTypeId = this.scenarioContext.Get<Dictionary<string, Guid>>("EnumDictionary")["LeadNegotiator"];
            Guid leadNegotiatorId = this.fixture.DataContext.Users.First().Id;
            var activityNegotiatorList = new List<ActivityUser>
            {
                new ActivityUser { UserId = leadNegotiatorId, UserTypeId = userTypeId }
            };

            var activity = new Activity
            {
                PropertyId = propertyId,
                ActivityTypeId = activityTypeId,
                ActivityStatusId = activityStatusId,
                CreatedDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
                Contacts = new List<Contact>(),
                Attachments = new List<Attachment>(),
                ActivityUsers = activityNegotiatorList
            };

            this.fixture.DataContext.Activities.Add(activity);
            this.fixture.DataContext.SaveChanges();

            this.scenarioContext.Set(activity, "Activity");
        }

        [When(@"User creates activity for given (.*) property id using api")]
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

            Guid leadNegotiatorId = this.fixture.DataContext.Users.First().Id;

            var activityCommand = new CreateActivityCommand
            {
                PropertyId = propertyId,
                ActivityStatusId = activityStatusId,
                ContactIds = vendors,
                ActivityTypeId = activityTypeId,
                LeadNegotiatorId = leadNegotiatorId
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
        public void UpdateActivitySaleValuation(string id, string status, Table table)
        {
            string requestUrl = $"{ApiUrl}";

            var updateActivityCommand = table.CreateInstance<UpdateActivityCommand>();
            var activityFromDatabase = this.scenarioContext.Get<Activity>("Activity");

            updateActivityCommand.Id = id.Equals("latest") ? activityFromDatabase.Id : new Guid(id);
            updateActivityCommand.ActivityTypeId = activityFromDatabase.ActivityTypeId;
            updateActivityCommand.LeadNegotiatorId = activityFromDatabase.ActivityUsers.First().UserId;

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
        public void GetActivityWithId(string activityId)
        {
            this.GetActivityResponse(activityId.Equals("latest")
                ? this.scenarioContext.Get<Activity>("Activity").Id.ToString()
                : activityId);
        }

        [When(@"User gets activities")]
        public void GetAllActivities()
        {
            this.GetActivityResponse();
        }

        [Then(@"Activities list should be the same as in database")]
        public void CheckActivitiesList()
        {
            var propertyFromResponse = JsonConvert.DeserializeObject<Property>(this.scenarioContext.GetResponseContent());

            Activity activitiesFromDatabase =
                this.fixture.DataContext.Properties.Single(prop => prop.Id.Equals(propertyFromResponse.Id)).Activities.First();

            AssertionOptions.AssertEquivalencyUsing(
                options => options.Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation)).WhenTypeIs<DateTime>());

            propertyFromResponse.Activities.ToList()[0].ShouldBeEquivalentTo(activitiesFromDatabase, options => options
                .Excluding(x => x.Property)
                .Excluding(x => x.ActivityStatus)
                .Excluding(x => x.ActivityType)
                .Excluding(x => x.Attachments)
                .Excluding(x => x.ActivityUsers));
        }

        [Then(@"Activity details should be the same as already added")]
        public void CompareActivities()
        {
            var activity = JsonConvert.DeserializeObject<Activity>(this.scenarioContext.GetResponseContent());
            Activity actualActivity = this.fixture.DataContext.Activities.Single(x => x.Id.Equals(activity.Id));

            actualActivity.ShouldBeEquivalentTo(activity, options => options
                .Excluding(x => x.Property)
                .Excluding(x => x.ActivityStatus)
                .Excluding(x => x.ActivityType)
                .Excluding(x => x.Attachments)
                .Excluding(x => x.ActivityUsers));

            actualActivity.ActivityStatus.Code.ShouldBeEquivalentTo("PreAppraisal");
            actualActivity.ActivityUsers.Should().Equal(activity.ActivityUsers, (c1, c2) =>
                c1.ActivityId == c2.ActivityId &&
                c1.UserTypeId == c2.UserTypeId &&
                c1.UserId == c2.UserId);
        }

        [Then(@"Retrieved activity should have expected viewing")]
        public void CheckActivityViewing()
        {
            var activity = JsonConvert.DeserializeObject<Activity>(this.scenarioContext.GetResponseContent());
            Viewing viewing = this.fixture.DataContext.Viewing.Single(x => x.ActivityId.Equals(activity.Id));

            activity.Viewings.Single().ShouldBeEquivalentTo(viewing, options => options
                .Excluding(v => v.Activity)
                .Excluding(v => v.Requirement)
                .Excluding(v => v.Negotiator));
        }

        [Then(@"Retrieved activity should have expected offer")]
        public void CheckActivityOffer()
        {
            var activity = JsonConvert.DeserializeObject<Activity>(this.scenarioContext.GetResponseContent());
            Offer offer = this.fixture.DataContext.Offer.Single(x => x.ActivityId.Equals(activity.Id));

            activity.Offers.Single().ShouldBeEquivalentTo(offer, options => options
                .Excluding(o => o.Activity)
                .Excluding(o => o.Requirement)
                .Excluding(o => o.Negotiator)
                .Excluding(o => o.Status));
        }

        [Then(@"Retrieved activities should be the same as in database")]
        public void CheckActivities(Table table)
        {
            var activity = JsonConvert.DeserializeObject<List<ActivitiesQueryResult>>(this.scenarioContext.GetResponseContent());
            List<ActivitiesQueryResult> actualActivity = table.CreateSet<ActivitiesQueryResult>().ToList();
            actualActivity.First().Id = this.scenarioContext.Get<Activity>("Activity").Id;

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
