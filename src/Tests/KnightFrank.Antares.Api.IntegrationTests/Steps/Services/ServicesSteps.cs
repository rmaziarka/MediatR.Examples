namespace KnightFrank.Antares.Api.IntegrationTests.Steps.Services
{
    using System;
    using System.Linq;
    using System.Net.Http;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Dal.Model.Enum;

    using TechTalk.SpecFlow;

    [Binding]
    public class ServicesSteps
    {
        private const string UploadApiUrl = "/api/services/attachment/upload";
        private const string DownloadApiUrl = "/api/services/attachment/download";

        private BaseTestClassFixture fixture;
        private ScenarioContext scenarioContext;

        public ServicesSteps(BaseTestClassFixture fixture, ScenarioContext scenarioContext)
        {
            this.fixture = fixture;
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            this.scenarioContext = scenarioContext;
        }

        [When(@"User retrieves url for activity attachment upload for (.*) entity reference id")]
        public void WhenUserRetrievesUrlForActivityAttachmentUploadForEntityReferenceId(string entityReferenceId)
        {
            var documentTypeId = this.scenarioContext.Get<Guid>("ActivityDocumentTypeId");

            var localeIsoCode = "en";
            var filename = "file.pdf";

            string activityUpload =
                $"/activity?documentTypeId={documentTypeId}&localeIsoCode={localeIsoCode}&entityReferenceId={entityReferenceId}&filename={filename}";

            HttpResponseMessage httpResponseMessage = this.fixture.SendGetRequest(UploadApiUrl + activityUpload);
            this.scenarioContext.SetHttpResponseMessage(httpResponseMessage);
        }

        [When(@"User retrieves url for activity attachment download for (.*) entity reference id")]
        public void WhenUserRetrievesUrlForActivityAttachmentDownloadForEntityReferenceId(string entityReferenceId)
        {
            var documentTypeId = this.scenarioContext.Get<Guid>("ActivityDocumentTypeId");
            Guid externalDocumentId = Guid.NewGuid();
            var localeIsoCode = "en";
            var filename = "file.pdf";

            string activityUpload =
                $"/activity?documentTypeId={documentTypeId}&localeIsoCode={localeIsoCode}&entityReferenceId={entityReferenceId}&filename={filename}&externalDocumentId={externalDocumentId}";

            HttpResponseMessage httpResponseMessage = this.fixture.SendGetRequest(DownloadApiUrl + activityUpload);
            this.scenarioContext.SetHttpResponseMessage(httpResponseMessage);
        }

    }
}