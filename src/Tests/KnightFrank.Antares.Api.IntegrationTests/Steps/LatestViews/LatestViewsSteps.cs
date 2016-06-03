namespace KnightFrank.Antares.Api.IntegrationTests.Steps.LatestViews
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Net.Http;

    using FluentAssertions;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.Dal.Model.Common;
    using KnightFrank.Antares.Dal.Model.LatestView;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Domain.LatestView.Commands;
    using KnightFrank.Antares.Domain.LatestView.QueryResults;

    using Newtonsoft.Json;

    using TechTalk.SpecFlow;

    using Xunit;

    [Binding]
    public class LatestViewsSteps : IClassFixture<BaseTestClassFixture>
    {
        private const string ApiUrl = "/api/latestviews";
        private readonly BaseTestClassFixture fixture;
        private readonly int maxLatestViews = int.Parse(ConfigurationManager.AppSettings["MaxLatestViews"]);

        private readonly ScenarioContext scenarioContext;

        public LatestViewsSteps(BaseTestClassFixture fixture, ScenarioContext scenarioContext)
        {
            this.fixture = fixture;
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            this.scenarioContext = scenarioContext;
        }

        [Given(@"Property is added to latest views")]
        public void AddToLatestViews()
        {
            Guid propertyId = this.scenarioContext.Get<Property>("AddedProperty").Id;
            List<Guid> propertiesIds = this.scenarioContext.ContainsKey("PropertiesIds")
                ? this.scenarioContext.Get<List<Guid>>("PropertiesIds")
                : new List<Guid>();

            propertiesIds.Add(propertyId);

            var details = new LatestView
            {
                CreatedDate = DateTime.UtcNow,
                UserId = this.fixture.DataContext.Users.First().Id,
                EntityId = propertyId,
                EntityType = EntityTypeEnum.Property
            };

            this.fixture.DataContext.LatestView.Add(details);
            this.fixture.DataContext.SaveChanges();
            this.scenarioContext["PropertiesIds"] = propertiesIds;
        }

        [When(@"User creates latest view")]
        public void CreateLatestView()
        {
            Guid propertyId = this.scenarioContext.Get<Property>("AddedProperty").Id;
            var details = new CreateLatestViewCommand
            {
                EntityType = EntityTypeEnum.Property,
                EntityId = propertyId
            };

            this.CreateLatestView(details);
        }

        [When(@"User creates latest view using invalid entity type")]
        public void CreateLatestViewWithInvalidEnum()
        {
            var details = new CreateLatestViewCommand
            {
                EntityType = (EntityTypeEnum)1000,
                EntityId = Guid.NewGuid()
            };

            this.CreateLatestView(details);
        }

        [When(@"User gets latest viewed entities")]
        public void CreateLatestViews()
        {
            this.GetLatestViews();
        }

        [Then(@"Latest viewed properties details should match viewed properties")]
        public void CheckLatestsViewedProperties()
        {
            List<Guid> propertiesIds = this.scenarioContext.Get<List<Guid>>("PropertiesIds").ToList();
            propertiesIds.Reverse();
            propertiesIds = propertiesIds.Distinct().Take(this.maxLatestViews).ToList();

            List<Guid> addressesIds =
                propertiesIds.Select(id => this.fixture.DataContext.Properties.Single(p => p.Id.Equals(id)).AddressId).ToList();
            List<Address> expectedAddresses =
                addressesIds.Select(id => this.fixture.DataContext.Addresses.Single(a => a.Id.Equals(id))).ToList();

            List<LatestViewData> response =
                JsonConvert.DeserializeObject<List<LatestViewQueryResultItem>>(this.scenarioContext.GetResponseContent())[0].List.ToList();
            List<Address> currentAddresses =
                response.Select(data => JsonConvert.DeserializeObject<Address>(data.Data.ToString())).ToList();

            expectedAddresses.ShouldAllBeEquivalentTo(currentAddresses, opt => opt
                .Excluding(a => a.Country));

            List<LatestView> latestViews = this.fixture.DataContext.LatestView.Select(r => r).GroupBy(lv => lv.EntityId)
                                               .Select(gLv => gLv.OrderByDescending(lv => lv.CreatedDate).FirstOrDefault())
                                               .OrderByDescending(lv => lv.CreatedDate).Take(this.maxLatestViews).ToList();

            latestViews.Should().Equal(response, (v1, v2) => v1.CreatedDate.Equals(v2.CreatedDate));
        }

        [Then(@"Retrieved latest view should contain property")]
        public void CheckLatestsViewedProperty()
        {
            Guid propertyId = this.scenarioContext.Get<Property>("AddedProperty").Id;

            List<LatestViewData> response =
                JsonConvert.DeserializeObject<List<LatestViewQueryResultItem>>(this.scenarioContext.GetResponseContent())[0].List.ToList();

            response.Should().HaveCount(1);

            var currentAddress = JsonConvert.DeserializeObject<Address>(response[0].Data.ToString());

            Guid addressId = this.fixture.DataContext.Properties.Single(p => p.Id.Equals(propertyId)).AddressId;
            Address expectedAddress = this.fixture.DataContext.Addresses.Single(a => a.Id.Equals(addressId));

            expectedAddress.ShouldBeEquivalentTo(currentAddress, opt => opt.Excluding(a => a.Country));

            LatestView latestView = this.fixture.DataContext.LatestView.First();

            latestView.CreatedDate.Should().Be(response[0].CreatedDate);
        }

        private void CreateLatestView(CreateLatestViewCommand command)
        {
            string requestUrl = $"{ApiUrl}";
            HttpResponseMessage response = this.fixture.SendPostRequest(requestUrl, command);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        private void GetLatestViews()
        {
            string requestUrl = $"{ApiUrl}";
            HttpResponseMessage response = this.fixture.SendGetRequest(requestUrl);
            this.scenarioContext.SetHttpResponseMessage(response);
        }
    }
}
