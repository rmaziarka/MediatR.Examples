namespace KnightFrank.Antares.Api.IntegrationTests.Steps.Property
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
    using KnightFrank.Antares.Domain.Property.Commands;

    using Newtonsoft.Json;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class AttachmentsSteps
    {
        private const string ApiUrl = "/api/properties";
        private readonly BaseTestClassFixture fixture;

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

        [Given(@"Property attachment for (.*) with following data exists in database")]
        public void CreateAttachmentForActivityInDatabase(string documentType, Table table)
        {
            var attachment = table.CreateInstance<Attachment>();
            attachment.DocumentTypeId = this.scenarioContext.Get<Dictionary<string, Guid>>("EnumDictionary")[documentType];
            attachment.CreatedDate = DateTime.Now;
            attachment.LastModifiedDate = DateTime.Now;
            attachment.UserId = this.fixture.DataContext.Users.First().Id;

            Guid propertyId = this.scenarioContext.Get<Property>("Property").Id;
            Property property = this.fixture.DataContext.Properties.Single(x => x.Id.Equals(propertyId));
            property.Attachments.Add(attachment);
            this.fixture.DataContext.SaveChanges();
        }

        [When(@"User uploads property attachment for (.*) property id for (.*) with following data")]
        public void WhenUserUploadsPropertyAttachmentForPropertyIdForWithFollowingData(string propertyId, string documentType,
            Table table)
        {
            if (propertyId.Equals("latest"))
            {
                propertyId = this.scenarioContext.Get<Property>("Property").Id.ToString();
            }

            string requestUrl = $"{ApiUrl}/{propertyId}/attachments";
            var createAttachment = table.CreateInstance<CreateAttachment>();

            createAttachment.DocumentTypeId = this.scenarioContext.Get<Dictionary<string, Guid>>("EnumDictionary")[documentType];
            createAttachment.UserId = this.fixture.DataContext.Users.First().Id;

            var createPropertyAttachmentCommand = new CreatePropertyAttachmentCommand
            {
                EntityId = propertyId.Equals(string.Empty) ? new Guid() : new Guid(propertyId),
                Attachment = createAttachment
            };

            HttpResponseMessage response = this.fixture.SendPostRequest(requestUrl, createPropertyAttachmentCommand);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [Then(@"Retrieved property should have expected attachments")]
        public void ThenRetrievedPropertyShouldHaveExpectedAttachments()
        {
            var property = JsonConvert.DeserializeObject<Property>(this.scenarioContext.GetResponseContent());
            Property actualProperty = this.fixture.DataContext.Properties.Single(x => x.Id.Equals(property.Id));

            actualProperty.Attachments.Should().Equal(property.Attachments, (c1, c2) =>
                c1.DocumentTypeId.Equals(c2.DocumentTypeId) &&
                c1.ExternalDocumentId.Equals(c2.ExternalDocumentId) &&
                c1.FileName.Equals(c2.FileName) &&
                c1.Size.Equals(c2.Size) &&
                c1.UserId.Equals(c2.UserId));
        }
    }
}
