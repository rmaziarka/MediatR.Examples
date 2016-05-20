namespace KnightFrank.Antares.Api.IntegrationTests.Steps.Property
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Domain.AreaBreakdown.Commands;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    using Xunit;

    [Binding]
    public class AreaBreakdownSteps
    {
        private const string ApiUrl = "/api/properties";
        private readonly BaseTestClassFixture fixture;

        private readonly ScenarioContext scenarioContext;
        private List<Area> areaList;
        private Guid propertyId;

        public AreaBreakdownSteps(BaseTestClassFixture fixture, ScenarioContext scenarioContext)
        {
            this.fixture = fixture;
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            this.scenarioContext = scenarioContext;
        }

        [Given(@"Following propery areas breakdown are defined")]
        public void GivenFollowingProperyAreasBreakdownAreDefined(Table table)
        {
            this.areaList = table.CreateSet<Area>().ToList();
        }

        [When(@"User creates defined property area breakdown for (.*) property")]
        public void WhenUserCreatesDefinedPropertyAreaBreakdown(string property)
        {
            this.propertyId = property.Equals("latest") ? this.scenarioContext.Get<Guid>("AddedPropertyId") : new Guid(property);

            string url = $"{ApiUrl}/{this.propertyId}/areabreakdown";
            var command = new CreateAreaBreakdownCommand
            {
                PropertyId = this.propertyId,
                Areas = this.areaList
            };
            HttpResponseMessage response = this.fixture.SendPostRequest(url, command);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [Then(@"Added property area breakdowns exists in data base")]
        public void ThenAddedPropertyAreaBreakdownsExistsInDataBase()
        {
            List<PropertyAreaBreakdown> currentPropertyAreaBreakdownList =
                this.fixture.DataContext.Properties.Single(x => x.Id.Equals(this.propertyId)).PropertyAreaBreakdowns.ToList();

            for (var i = 0; i < currentPropertyAreaBreakdownList.Count; i++)
            {
                Assert.True(currentPropertyAreaBreakdownList[i].Order == i &&
                            currentPropertyAreaBreakdownList[i].Name.Equals(this.areaList[i].Name) &&
                            currentPropertyAreaBreakdownList[i].Size.Equals(this.areaList[i].Size));
            }
        }
    }
}
