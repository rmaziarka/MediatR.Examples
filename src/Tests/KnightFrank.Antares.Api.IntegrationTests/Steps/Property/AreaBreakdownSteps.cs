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

        [Given(@"Following property area breakdown is defined")]
        public void GivenFollowingProperyAreasBreakdownAreDefined(Table table)
        {
            this.areaList = table.CreateSet<Area>().ToList();
        }

        [Given(@"Following property area breakdown is in database")]
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
            PropertyAreaBreakdown propertyAreaBreakdown = this.GetPropertyAreaBreakdown(property, name);
            string url = $"{ApiUrl}/{this.propertyId}/areabreakdown/order";

            var command = new UpdateAreaBreakdownOrderCommand
            {
                PropertyId = this.propertyId,
                AreaId = propertyAreaBreakdown?.Id ?? Guid.NewGuid(),
                Order = order
            };

            HttpResponseMessage response = this.fixture.SendPutRequest(url, command);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [When(@"User updates (.*) name and (.*) size property area breakdown with (.*) for (.*) property")]
        public void WhenUserUpdatesAndPropertyAreaBreakdownWithForProperty(string updatedName, int updatedPropertySize, string name,
            string property)
        {
            PropertyAreaBreakdown propertyAreaBreakdown = this.GetPropertyAreaBreakdown(property, name);
            string url = $"{ApiUrl}/{this.propertyId}/areabreakdown";

            var command = new UpdateAreaBreakdownCommand
            {
                PropertyId = this.propertyId,
                Id = this.GetPropertyAreaBreakdown(property, name)?.Id ?? Guid.NewGuid(),
                Name = updatedName,
                Size = updatedPropertySize
            };

            HttpResponseMessage response = this.fixture.SendPutRequest(url, command);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [Then(@"Added property area breakdowns should exist in data base")]
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

        [Then(@"Returned property area breakdowns should be as expected")]
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

        [Then(@"Property area breakdown should be updated")]
        public void ThenPropertyAreaBreakdownsShouldHaveNewOrder(Table table)
        {
            List<PropertyAreaBreakdown> currentPropertyAreaBreakdownList =
                this.fixture.DataContext.Properties.Single(x => x.Id.Equals(this.propertyId)).PropertyAreaBreakdowns.ToList();

            IEnumerable<PropertyAreaBreakdown> expectedAreaBreakdown = table.CreateSet<PropertyAreaBreakdown>();

            currentPropertyAreaBreakdownList.Should()
                                            .Equal(expectedAreaBreakdown,
                                                (c1, c2) =>
                                                    c1.Name.Equals(c2.Name) && c1.Order.Equals(c2.Order) && c1.Size.Equals(c2.Size));
        }

        private PropertyAreaBreakdown GetPropertyAreaBreakdown(string property, string name)
        {
            this.propertyId = property.Equals("latest") ? this.scenarioContext.Get<Guid>("AddedPropertyId") : new Guid(property);

            Property areaBreakdownProperty = this.fixture.DataContext.Properties.SingleOrDefault(x => x.Id.Equals(this.propertyId));

            return areaBreakdownProperty?.PropertyAreaBreakdowns.SingleOrDefault(area => area.Name.Equals(name));
        }
    }
}
