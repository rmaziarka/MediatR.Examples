namespace KnightFrank.Antares.Api.IntegrationTests.Steps.Requierement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    using FluentAssertions;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Dal.Model.Attachment;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Domain.Attachment.Commands;
    using KnightFrank.Antares.Domain.Requirement.Commands;

    using Newtonsoft.Json;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class AttachmentsSteps
    {
        private const string ApiUrl = "/api/requirements/{0}/attachments";
        private readonly BaseTestClassFixture fixture;
        private readonly DateTime date = DateTime.UtcNow;
        private readonly ScenarioContext scenarioContext;

        public AttachmentsSteps(BaseTestClassFixture fixture, ScenarioContext scenarioContext)
        {
            this.fixture = fixture;
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            this.scenarioContext = scenarioContext;
        }

        [Given(@"Requirement attachment for (.*) with following data exists in database")]
        public void AddAttachment(string documentType, Table table)
        {
            var attachment = table.CreateInstance<Attachment>();

            attachment.DocumentTypeId = this.scenarioContext.Get<Dictionary<string, Guid>>("EnumDictionary")[documentType];
            attachment.CreatedDate = this.date;
            attachment.LastModifiedDate = this.date;
            attachment.UserId = this.fixture.DataContext.Users.First().Id;

            Guid requirementId = this.scenarioContext.Get<Requirement>("Requirement").Id;
            Requirement requirement = this.fixture.DataContext.Requirements.Single(x => x.Id.Equals(requirementId));

            requirement.Attachments.Add(attachment);
            this.fixture.DataContext.SaveChanges();
        }

        [When(@"User uploads requirement attachment for (.*) requirement id for (.*) with following data")]
        public void UploadAttachment(string requirementId, string documentType, Table table)
        {
            if (requirementId.Equals("latest"))
            {
                requirementId = this.scenarioContext.Get<Requirement>("Requirement").Id.ToString();
            }

            string requestUrl = string.Format($"{ApiUrl}", requirementId);

            var createAttachment = table.CreateInstance<CreateAttachment>();

            createAttachment.DocumentTypeId = this.scenarioContext.Get<Dictionary<string, Guid>>("EnumDictionary")[documentType];
            createAttachment.UserId = this.fixture.DataContext.Users.First().Id;

            var details = new CreateRequirementAttachmentCommand
            {
                EntityId = requirementId.Equals(string.Empty) ? Guid.NewGuid() : new Guid(requirementId),
                Attachment = createAttachment
            };

            HttpResponseMessage response = this.fixture.SendPostRequest(requestUrl, details);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [Then(@"Retrieved requirement should have expected attachments")]
        public void CheckAttachment()
        {
            var requirement = JsonConvert.DeserializeObject<Requirement>(this.scenarioContext.GetResponseContent());
            Requirement actualRequirement = this.fixture.DataContext.Requirements.Single(x => x.Id.Equals(requirement.Id));

            actualRequirement.Attachments.Should().Equal(requirement.Attachments, (c1, c2) =>
                c1.DocumentTypeId.Equals(c2.DocumentTypeId) &&
                c1.ExternalDocumentId.Equals(c2.ExternalDocumentId) &&
                c1.FileName.Equals(c2.FileName) &&
                c1.Size.Equals(c2.Size) &&
                c1.UserId.Equals(c2.UserId));
        }
    }
}
