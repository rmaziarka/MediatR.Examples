namespace KnightFrank.Antares.Api.IntegrationTests.Steps.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Property.Activities;

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

        [When(@"User retrieves url for activity attachment upload for (.*) and (.*)")]
        public void WhenUserRetrievesUrlForActivityAttachmentUploadForEntityReferenceId(string filename, string activityDocumentTypeCode)
        {
            var documentTypeId = this.scenarioContext.Get<Dictionary<string, Guid>>("EnumDictionary")[activityDocumentTypeCode];
            var entityReferenceId = this.scenarioContext.Get<Activity>("Activity").Id;

            var localeIsoCode = "en";

            string activityUpload =
                $"/activity?documentTypeId={documentTypeId}&localeIsoCode={localeIsoCode}&entityReferenceId={entityReferenceId}&filename={filename}";

            HttpResponseMessage httpResponseMessage = this.fixture.SendGetRequest(UploadApiUrl + activityUpload);
            this.scenarioContext.SetHttpResponseMessage(httpResponseMessage);
        }

        [When(@"User retrieves url for activity attachment download for (.*) and (.*)")]
        public void WhenUserRetrievesUrlForActivityAttachmentDownloadForEntityReferenceId(string filename, string activityDocumentTypeCode)
        {
            var documentTypeId = this.scenarioContext.Get<Dictionary<string, Guid>>("EnumDictionary")[activityDocumentTypeCode];
            var entityReferenceId = this.scenarioContext.Get<Activity>("Activity").Id;

            Guid externalDocumentId = Guid.NewGuid();
            var localeIsoCode = "en";

            string activityUpload =
                $"/activity?documentTypeId={documentTypeId}&localeIsoCode={localeIsoCode}&entityReferenceId={entityReferenceId}&filename={filename}&externalDocumentId={externalDocumentId}";

            HttpResponseMessage httpResponseMessage = this.fixture.SendGetRequest(DownloadApiUrl + activityUpload);
            this.scenarioContext.SetHttpResponseMessage(httpResponseMessage);
        }

    }
}