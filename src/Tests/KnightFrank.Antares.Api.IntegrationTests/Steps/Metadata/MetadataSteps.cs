namespace KnightFrank.Antares.Api.IntegrationTests.Steps.Metadata
{
    using System;
    using System.Net.Http;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;

    using TechTalk.SpecFlow;

    using Xunit;

    [Binding]
    public class MetadataSteps : IClassFixture<BaseTestClassFixture>
    {
        private const string ApiUrl = "/api/metadata/{0}";
        private const string AttributesActivity = "attributes/activity?activityTypeId={0}&pageType={1}&propertyTypeId={2}";
        private readonly BaseTestClassFixture fixture;
        private readonly ScenarioContext scenarioContext;

        public MetadataSteps(BaseTestClassFixture fixture, ScenarioContext scenarioContext)
        {
            this.fixture = fixture;
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            this.scenarioContext = scenarioContext;
        }

        [When(@"User gets activity preview attributes")]
        public void GetActivityAttributes()
        {
            const string pageType = nameof(PageType.Create);
            Guid propertyTypeId = this.scenarioContext.Get<Property>("Property").PropertyTypeId;
            var activityTypeId = this.scenarioContext.Get<Guid>("ActivityTypeId");

            this.GetActivityAttributes(pageType, propertyTypeId.ToString(), activityTypeId.ToString());
        }

        [When(@"User gets activity preview attributes with invalid (.*) data")]
        public void GetActivityAttributesInvalidData(string data)
        {
            string pageType = data.Equals("page") ? "invalid" : nameof(PageType.Create);
            Guid propertyTypeId = data.Equals("propertyType")
                ? Guid.NewGuid()
                : this.scenarioContext.Get<Property>("Property").PropertyTypeId;

            Guid activityTypeId = data.Equals("activityType")
                ? Guid.NewGuid()
                : this.scenarioContext.Get<Guid>("ActivityTypeId");

            this.GetActivityAttributes(pageType, propertyTypeId.ToString(), activityTypeId.ToString());
        }

        [When(@"User gets activity preview attributes with empty (.*) data")]
        public void GetActivityAttributesEmptyData(string data)
        {
            const string pageType = nameof(PageType.Preview);
            Guid propertyTypeId = this.scenarioContext.Get<Property>("Property").PropertyTypeId;
            var activityTypeId = this.scenarioContext.Get<Guid>("ActivityTypeId");

            if (data.Equals("activity"))
            {
                this.GetActivityAttributes(pageType, propertyTypeId.ToString(), string.Empty);
            }
            else if (data.Equals("property"))
            {
                this.GetActivityAttributes(pageType, string.Empty, activityTypeId.ToString());
            }
            else if (data.Equals("page"))
            {
                this.GetActivityAttributes(string.Empty, propertyTypeId.ToString(), activityTypeId.ToString());
            }
        }

        private void GetActivityAttributes(string pageType, string propertyTypeId, string activityTypeId)
        {
            string attributes = string.Format(AttributesActivity, activityTypeId, pageType, propertyTypeId);
            string requestUrl = string.Format(ApiUrl, attributes);
            HttpResponseMessage response = this.fixture.SendPostRequest(requestUrl, null);
            this.scenarioContext.SetHttpResponseMessage(response);
        }
    }
}
