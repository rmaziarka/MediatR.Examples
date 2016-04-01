namespace KnightFrank.Antares.Api.IntegrationTests.Steps.Activities
{
    using System;
    using System.Linq;
    using System.Net.Http;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Domain.Activity.Commands;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class ActivitiesSteps
    {
        private const string ApiUrl = "/api/Activities";
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

        [Given(@"There is activity type id and activity status id defined")]
        public void GivenThereIsActivityTypeIdAndActivityStatusIdDefined(Guid id)
        {
            IQueryable<Contact> abc =
                this.fixture.DataContext.Ownerships.Where(x => x.PropertyId == id && x.SellDate == null)
                    .SelectMany(x => x.Contacts);
        }

        [When(@"User creates activity with following data")]
        public void WhenUserCreatesActivityWithActivityIdActivityStatusIdForPropertyId(Table table)
        {
            string requestUrl = $"{ApiUrl}";
            var activityCommand = table.CreateInstance<CreateActivityCommand>();

            //Guid propertyId = id.Equals("latest") ? this.scenarioContext.Get<Guid>("AddedPropertyId") : new Guid(id);

            HttpResponseMessage response = this.fixture.SendPostRequest(requestUrl, activityCommand);
            this.scenarioContext.SetHttpResponseMessage(response);
        }
    }
}
