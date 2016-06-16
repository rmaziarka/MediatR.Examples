namespace KnightFrank.Antares.Api.IntegrationTests.Steps.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;

    using TechTalk.SpecFlow;

    [Binding]
    public class ServicesSteps
    {
        private const string DownloadApiUrl = "/api/services/attachment/download";
        private const string LocaleIsoCode = "en";
        private const string UploadApiUrl = "/api/services/attachment/upload";

        private readonly BaseTestClassFixture fixture;
        private readonly ScenarioContext scenarioContext;
        private Guid documentTypeId;
        private Guid entityReferenceId;

        public ServicesSteps(BaseTestClassFixture fixture, ScenarioContext scenarioContext)
        {
            this.fixture = fixture;
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            this.scenarioContext = scenarioContext;
        }

        [When(@"User retrieves url for (Activity|Property) attachment upload for (.*) and (.*) code")]
        public void WhenUserRetrievesUrlForActivityAttachmentUploadForEntityReferenceId(string entity, string filename,
            string activityDocumentTypeCode)
        {
            this.GetEntity(entity, activityDocumentTypeCode);

            string activityUpload =
                $"/{entity.ToLower()}?documentTypeId={this.documentTypeId}&localeIsoCode={LocaleIsoCode}&entityReferenceId={this.entityReferenceId}&filename={filename}";

            HttpResponseMessage httpResponseMessage = this.fixture.SendGetRequest(UploadApiUrl + activityUpload);
            this.scenarioContext.SetHttpResponseMessage(httpResponseMessage);
        }

        [When(@"User retrieves url for (Activity|Property) attachment download for (.*) and (.*) code")]
        public void WhenUserRetrievesUrlForActivityAttachmentDownloadForEntityReferenceId(string entity, string filename,
            string activityDocumentTypeCode)
        {
            this.GetEntity(entity, activityDocumentTypeCode);

            Guid externalDocumentId = Guid.NewGuid();

            string activityUpload =
                $"/{entity.ToLower()}?documentTypeId={this.documentTypeId}&localeIsoCode={LocaleIsoCode}&entityReferenceId={this.entityReferenceId}&filename={filename}&externalDocumentId={externalDocumentId}";

            HttpResponseMessage httpResponseMessage = this.fixture.SendGetRequest(DownloadApiUrl + activityUpload);
            this.scenarioContext.SetHttpResponseMessage(httpResponseMessage);
        }

        private void GetEntity(string entity, string activityDocumentTypeCode)
        {
            this.documentTypeId = this.scenarioContext.Get<Dictionary<string, Guid>>("EnumDictionary")[activityDocumentTypeCode];

            switch (entity)
            {
                case "Activity":
                    this.entityReferenceId = this.scenarioContext.Get<Activity>("Activity").Id;
                    break;
                case "Property":
                    this.entityReferenceId = this.scenarioContext.Get<Property>("Property").Id;
                    break;
            }
        }
    }
}
