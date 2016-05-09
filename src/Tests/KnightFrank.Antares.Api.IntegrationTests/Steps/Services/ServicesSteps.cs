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
        private const string ApiUrl = "/api/services/attachment/upload";

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

        [When(@"User retrieves url for activity attachment upload for (.*) activity document type")]
        public void WhenUserRetrievesUrlForActivityAttachmentUploadForInvalidCodeActivityDocumentType(string documentTypeCode)
        {
            EnumTypeItem documentType = this.fixture.DataContext.EnumTypeItems.SingleOrDefault(x => x.Code == documentTypeCode);

            Guid documentTypeId = documentType?.Id ?? Guid.NewGuid();
            string localeIsoCode = "en";
            Guid entityReferenceId = Guid.NewGuid();
            string filename = "file.pdf";

            string activityUpload =
                $"/activity?documentTypeId={documentTypeId}&localeIsoCode={localeIsoCode}&entityReferenceId={entityReferenceId}&filename={filename}";

            HttpResponseMessage httpResponseMessage = this.fixture.SendGetRequest(ApiUrl + activityUpload);
            this.scenarioContext.SetHttpResponseMessage(httpResponseMessage);
        }
    }
}