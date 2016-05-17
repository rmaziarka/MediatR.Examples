namespace KnightFrank.Antares.Api.IntegrationTests.Steps.Activities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    using FluentAssertions;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Dal.Model.Attachment;
    using KnightFrank.Antares.Dal.Model.Common;
    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.Activity.QueryResults;
    using KnightFrank.Antares.Domain.Attachment.Commands;

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
            Guid leadNegotiatorId = this.fixture.DataContext.Users.First().Id;
            var activityNegotiatorList = new List<ActivityUser>
            {
                new ActivityUser { UserId = leadNegotiatorId, UserType = UserTypeEnum.LeadNegotiator }
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

        [Given(@"Attachment for (.*) with following data exists in data base")]
        public void CreateAttachmentForActivityInDatabase(string documentType, Table table)
        {
            var attachment = table.CreateInstance<Attachment>();
            attachment.DocumentTypeId = this.scenarioContext.Get<Dictionary<string, Guid>>("EnumDictionary")[documentType];
            attachment.CreatedDate = DateTime.Now;
            attachment.LastModifiedDate = DateTime.Now;
            attachment.UserId = this.scenarioContext.Get<Guid>("NegotiatorId");

            Guid activityId = this.scenarioContext.Get<Activity>("Activity").Id;
            Activity activity = this.fixture.DataContext.Activities.Single(x => x.Id.Equals(activityId));
            activity.Attachments.Add(attachment);
            this.fixture.DataContext.SaveChanges();
        }

        [When(@"User updates activity (.*) id and (.*) status with following sale valuation")]
        public void UpdateActivitySaleValuation(string id, string status, Table table)
        {
            string requestUrl = $"{ApiUrl}";

            var updateActivityCommand = table.CreateInstance<UpdateActivityCommand>();
            var activityFromDatabase = this.scenarioContext.Get<Activity>("Activity");

            updateActivityCommand.Id = id.Equals("latest") ? activityFromDatabase.Id : new Guid(id);
            updateActivityCommand.ActivityTypeId = activityFromDatabase.ActivityTypeId;
            //TODO: implement better setting of lead negotiator id when implementing update negotiator test cases
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

        [When(@"I upload attachment for (.*) activity id for (.*) with following data")]
        public void UploadAttachmentForActivity(string activityId, string documentType, Table table)
        {
            if (activityId.Equals("latest"))
            {
                activityId = this.scenarioContext.Get<Activity>("Activity").Id.ToString();
            }

            string requestUrl = $"{ApiUrl}/{activityId}/attachments";

            var createAttachment = table.CreateInstance<CreateAttachment>();

            createAttachment.DocumentTypeId = this.scenarioContext.Get<Dictionary<string, Guid>>("EnumDictionary")[documentType];
            createAttachment.UserId = this.scenarioContext.Get<Guid>("NegotiatorId");

            var createActivityAttachmentCommand = new CreateActivityAttachmentCommand
            {
                ActivityId = activityId.Equals(string.Empty) ? new Guid() : new Guid(activityId),
                Attachment = createAttachment
            };

            HttpResponseMessage response = this.fixture.SendPostRequest(requestUrl, createActivityAttachmentCommand);
            this.scenarioContext.SetHttpResponseMessage(response);
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

        [Then(@"Created Activity is saved in database")]
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
                c1.UserType == c2.UserType &&
                c1.UserId == c2.UserId);
        }

        [Then(@"Retrieved activity should be same as in database")]
        public void CheckActivity()
        {
            var activity = JsonConvert.DeserializeObject<Activity>(this.scenarioContext.GetResponseContent());
            Activity actualActivity = this.fixture.DataContext.Activities.Single(x => x.Id.Equals(activity.Id));

            actualActivity.ShouldBeEquivalentTo(activity, options => options
                .Excluding(a => a.ActivityStatus)
                .Excluding(a => a.Contacts)
                .Excluding(a => a.Property)
                .Excluding(a => a.ActivityType)
                .Excluding(a => a.Attachments)
                .Excluding(a => a.ActivityUsers)
                .Excluding(a => a.Viewings));

            actualActivity.ActivityUsers.Should().Equal(activity.ActivityUsers, (c1, c2) =>
                c1.ActivityId == c2.ActivityId &&
                c1.UserType == c2.UserType &&
                c1.UserId == c2.UserId);
        }

        [Then(@"Retrieved activity should have expected attachments")]
        public void CheckActivityAttachments()
        {
            var activity = JsonConvert.DeserializeObject<Activity>(this.scenarioContext.GetResponseContent());
            Activity actualActivity = this.fixture.DataContext.Activities.Single(x => x.Id.Equals(activity.Id));

            actualActivity.Attachments.Should().Equal(activity.Attachments, (c1, c2) =>
                c1.DocumentTypeId.Equals(c2.DocumentTypeId) &&
                c1.ExternalDocumentId.Equals(c2.ExternalDocumentId) &&
                c1.FileName.Equals(c2.FileName) &&
                c1.Size.Equals(c2.Size) &&
                c1.UserId.Equals(c2.UserId));
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
