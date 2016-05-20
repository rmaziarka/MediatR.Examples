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
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.Attachment.Commands;

    using Newtonsoft.Json;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class AttachmentSteps
    {
        private const string ApiUrl = "/api/activities";
        private readonly BaseTestClassFixture fixture;

        private readonly ScenarioContext scenarioContext;

        public AttachmentSteps(BaseTestClassFixture fixture, ScenarioContext scenarioContext)
        {
            this.fixture = fixture;
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            this.scenarioContext = scenarioContext;
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
    }
}
