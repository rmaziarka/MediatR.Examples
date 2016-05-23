namespace KnightFrank.Antares.Api.IntegrationTests.Steps.Property
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    using FluentAssertions;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Domain.AreaBreakdown.Commands;

    using Newtonsoft.Json;

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
        private List<PropertyAreaBreakdown> propertyAreaBreakdownList;
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

        [Given(@"Following propery areas breakdown are defined and put in data base")]
        public void GivenFollowingProperyAreasBreakdownAreDefinedAndPutInDb(Table table)
        {
            this.propertyAreaBreakdownList = table.CreateSet<PropertyAreaBreakdown>().ToList();

            foreach (PropertyAreaBreakdown area in this.propertyAreaBreakdownList)
            {
                area.PropertyId = this.scenarioContext.Get<Guid>("AddedPropertyId");
            }

            this.fixture.DataContext.PropertyAreaBreakdown.AddRange(this.propertyAreaBreakdownList);
            this.fixture.DataContext.SaveChanges();
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

        [When(@"User sets (.*) order for (.*) property area breakdown for (.*) property")]
        public void WhenUserSetsOrderForPropertyAreaBreakdown(int order, string name, string property)
        {
            this.propertyId = property.Equals("latest") ? this.scenarioContext.Get<Guid>("AddedPropertyId") : new Guid(property);
            string url = $"{ApiUrl}/{this.propertyId}/areabreakdown/order";

            Property areaBreakdownProperty = this.fixture.DataContext.Properties.SingleOrDefault(x => x.Id.Equals(this.propertyId));

            PropertyAreaBreakdown propertyAreaBreakdown =
                areaBreakdownProperty?.PropertyAreaBreakdowns.SingleOrDefault(area => area.Name.Equals(name));

            var command = new UpdateAreaBreakdownOrderCommand
            {
                PropertyId = this.propertyId,
                AreaId = propertyAreaBreakdown?.Id ?? Guid.NewGuid(),
                Order = order
            };

            HttpResponseMessage response = this.fixture.SendPutRequest(url, command);
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

        [Then(@"Returned property area breakdowns are as expected")]
        public void ThenReturnedPropertyAreaBreakdownsAreAsExpected()
        {
            var propertyFromResponse = JsonConvert.DeserializeObject<Property>(this.scenarioContext.GetResponseContent());

            propertyFromResponse.PropertyAreaBreakdowns.Should().Equal(this.propertyAreaBreakdownList, (c1, c2) =>
                c1.Name.Equals(c2.Name) &&
                c1.Size.Equals(c2.Size) &&
                c1.Order.Equals(c2.Order) &&
                c1.Id.Equals(c2.Id) &&
                c1.PropertyId.Equals(c2.PropertyId));
        }

        [Then(@"Property area breakdowns should have new order")]
        public void ThenPropertyAreaBreakdownsShouldHaveNewOrder(Table table)
        {
            List<PropertyAreaBreakdown> currentPropertyAreaBreakdownList =
                this.fixture.DataContext.Properties.Single(x => x.Id.Equals(this.propertyId)).PropertyAreaBreakdowns.ToList();

            IEnumerable<PropertyAreaBreakdown> expectedAreaBreakdown = table.CreateSet<PropertyAreaBreakdown>();

            currentPropertyAreaBreakdownList.Should()
                                            .Equal(expectedAreaBreakdown,
                                                (c1, c2) => c1.Name.Equals(c2.Name) && c1.Order.Equals(c2.Order) && c1.Size.Equals(c2.Size));
        }
    }
}
