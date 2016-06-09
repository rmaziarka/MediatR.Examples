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
    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.LatestView;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
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

        // TODO factory refactor
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
        public void AddPropertyToLatestViews()
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

        [Given(@"Activity is added to latest views")]
        public void AddActivityToLatestViews()
        {
            Guid activityId = this.scenarioContext.Get<Activity>("Activity").Id;
            List<Guid> activitiesIds = this.scenarioContext.ContainsKey("ActivitiesIds")
                ? this.scenarioContext.Get<List<Guid>>("ActivitiesIds")
                : new List<Guid>();

            activitiesIds.Add(activityId);

            var details = new LatestView
            {
                CreatedDate = DateTime.UtcNow,
                UserId = this.fixture.DataContext.Users.First().Id,
                EntityId = activityId,
                EntityType = EntityTypeEnum.Activity
            };

            this.fixture.DataContext.LatestView.Add(details);
            this.fixture.DataContext.SaveChanges();
            this.scenarioContext["ActivitiesIds"] = activitiesIds;
        }

        [Given(@"Requirement is added to latest views")]
        public void AddRequirementToLatestViews()
        {
            Guid requirementId = this.scenarioContext.Get<Requirement>("Requirement").Id;
            List<Guid> requirementsId = this.scenarioContext.ContainsKey("RequirementsId")
                ? this.scenarioContext.Get<List<Guid>>("RequirementsId")
                : new List<Guid>();

            requirementsId.Add(requirementId);

            var details = new LatestView
            {
                CreatedDate = DateTime.UtcNow,
                UserId = this.fixture.DataContext.Users.First().Id,
                EntityId = requirementId,
                EntityType = EntityTypeEnum.Requirement
            };

            this.fixture.DataContext.LatestView.Add(details);
            this.fixture.DataContext.SaveChanges();
            this.scenarioContext["RequirementsId"] = requirementsId;
        }

        [When(@"User adds (.*) to latest viewed entities using api")]
        public void CreateLatestView(string entity)
        {
            var details = new CreateLatestViewCommand();
            if (entity.ToLower().Equals("property"))
            {
                Guid propertyId = this.scenarioContext.Get<Property>("AddedProperty").Id;
                details.EntityId = propertyId;
                details.EntityType = EntityTypeEnum.Property;
            }
            else if (entity.ToLower().Equals("activity"))
            {
                Guid activityId = this.scenarioContext.Get<Activity>("Activity").Id;
                details.EntityId = activityId;
                details.EntityType = EntityTypeEnum.Activity;
            }
            else if (entity.ToLower().Equals("requirement"))
            {
                Guid requirementId = this.scenarioContext.Get<Requirement>("Requirement").Id;
                details.EntityId = requirementId;
                details.EntityType = EntityTypeEnum.Requirement;
            }

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

        [Then(@"Latest viewed details should match (.*) entities")]
        public void CheckLatestsViewedProperties(string entity)
        {
            var entitiesIds = new List<Guid>();
            var addressesIds = new List<Guid>();
            string entityTypeCode = string.Empty;

            if (entity.ToLower().Equals("property"))
            {
                entitiesIds = this.scenarioContext.Get<List<Guid>>("PropertiesIds").ToList();
                entitiesIds.Reverse();
                entitiesIds = entitiesIds.Distinct().Take(this.maxLatestViews).ToList();
                addressesIds =
                    entitiesIds.Select(id => this.fixture.DataContext.Properties.Single(p => p.Id.Equals(id)).AddressId).ToList();
                entityTypeCode = "Property";
            }
            else if (entity.ToLower().Equals("activity"))
            {
                entitiesIds = this.scenarioContext.Get<List<Guid>>("ActivitiesIds").ToList();
                entitiesIds.Reverse();
                entitiesIds = entitiesIds.Distinct().Take(this.maxLatestViews).ToList();
                addressesIds =
                    entitiesIds.Select(id => this.fixture.DataContext.Activities.Single(p => p.Id.Equals(id)).Property.AddressId)
                               .ToList();
                entityTypeCode = "Activity";
            }
            else if (entity.ToLower().Equals("requirement"))
            {
                entitiesIds = this.scenarioContext.Get<List<Guid>>("RequirementsId").ToList();
                entitiesIds.Reverse();
                entitiesIds = entitiesIds.Distinct().Take(this.maxLatestViews).ToList();
                entityTypeCode = "Requirement";
            }

            LatestViewQueryResultItem response =
                JsonConvert.DeserializeObject<List<LatestViewQueryResultItem>>(this.scenarioContext.GetResponseContent()).Single();

            response.EntityTypeCode.Should().Be(entityTypeCode);

            List<LatestViewData> data = response.List.ToList();

            if (entity.ToLower().Equals("property") || entity.ToLower().Equals("activity"))
            {
                List<Address> expectedAddresses =
                    addressesIds.Select(id => this.fixture.DataContext.Addresses.Single(a => a.Id.Equals(id))).ToList();
                List<Address> currentAddresses =
                    data.Select(d => JsonConvert.DeserializeObject<Address>(d.Data.ToString())).ToList();

                expectedAddresses.ShouldAllBeEquivalentTo(currentAddresses, opt => opt
                    .Excluding(a => a.Country));
            }
            else if (entity.ToLower().Equals("requirement"))
            {
                List<List<Contact>> currentContacts =
                    data.Select(d => JsonConvert.DeserializeObject<List<Contact>>(d.Data.ToString())).ToList();

                List<List<Contact>> expectedContacts =
                    entitiesIds.Select(
                        guid => this.fixture.DataContext.Requirements.Single(req => req.Id.Equals(guid)).Contacts.ToList())
                               .ToList();

                expectedContacts.ShouldAllBeEquivalentTo(currentContacts);
            }

            List<LatestView> latestViews = this.fixture.DataContext.LatestView.Select(r => r).GroupBy(lv => lv.EntityId)
                                               .Select(gLv => gLv.OrderByDescending(lv => lv.CreatedDate).FirstOrDefault())
                                               .OrderByDescending(lv => lv.CreatedDate).Take(this.maxLatestViews).ToList();

            latestViews.Should().Equal(data, (v1, v2) => v1.CreatedDate.Equals(v2.CreatedDate));
        }

        [Then(@"Retrieved latest view should contain (.*) entity")]
        public void CheckLatestsViewedEntity(string entity)
        {
            Guid entityId = Guid.Empty;
            Guid addressId = Guid.NewGuid();
            string entityTypeCode = string.Empty;

            if (entity.ToLower().Equals("property"))
            {
                entityId = this.scenarioContext.Get<Property>("AddedProperty").Id;
                addressId = this.fixture.DataContext.Properties.Single(p => p.Id.Equals(entityId)).AddressId;
                entityTypeCode = "Property";
            }
            else if (entity.ToLower().Equals("activity"))
            {
                entityId = this.scenarioContext.Get<Activity>("Activity").Id;
                addressId = this.fixture.DataContext.Activities.Single(p => p.Id.Equals(entityId)).Property.AddressId;
                entityTypeCode = "Activity";
            }
            else if (entity.ToLower().Equals("requirement"))
            {
                entityId = this.scenarioContext.Get<Requirement>("Requirement").Id;
                entityTypeCode = "Requirement";
            }

            LatestViewQueryResultItem response =
                JsonConvert.DeserializeObject<List<LatestViewQueryResultItem>>(this.scenarioContext.GetResponseContent()).Single();

            response.EntityTypeCode.Should().Be(entityTypeCode);

            List<LatestViewData> data = response.List.ToList();
            data.Should().HaveCount(1);

            if (entity.ToLower().Equals("activity") || entity.ToLower().Equals("property"))
            {
                var currentAddress = JsonConvert.DeserializeObject<Address>(data.Single().Data.ToString());
                Address expectedAddress = this.fixture.DataContext.Addresses.Single(a => a.Id.Equals(addressId));

                expectedAddress.ShouldBeEquivalentTo(currentAddress, opt => opt.Excluding(a => a.Country));
            }
            else if(entity.ToLower().Equals("requirement"))
            {
                var currentContacts = JsonConvert.DeserializeObject<List<Contact>>(data.Single().Data.ToString());
                List<Contact> expectedContacts = this.fixture.DataContext.Requirements.Single(req => req.Id.Equals(entityId)).Contacts.ToList();

                expectedContacts.Should().Equal(currentContacts,
                    (c1, c2) => c1.FirstName.Equals(c2.FirstName) &&
                                c1.Surname.Equals(c2.Surname) &&
                                c1.Title.Equals(c2.Title) &&
                                c1.Id.Equals(c2.Id));
            }

            LatestView latestView = this.fixture.DataContext.LatestView.Single();

            latestView.CreatedDate.Should().Be(data.Single().CreatedDate);
            latestView.EntityTypeString.Should().Be(entityTypeCode);
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
