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
    using KnightFrank.Antares.Dal.Model.Company;
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
        private readonly DateTime date = DateTime.UtcNow;
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
            Guid propertyId = this.scenarioContext.Get<Property>("Property").Id;
            List<Guid> propertiesIds = this.scenarioContext.ContainsKey("PropertiesIds")
                ? this.scenarioContext.Get<List<Guid>>("PropertiesIds")
                : new List<Guid>();

            propertiesIds.Add(propertyId);

            var details = new LatestView
            {
                CreatedDate = this.date,
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
                CreatedDate = this.date,
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
                CreatedDate = this.date,
                UserId = this.fixture.DataContext.Users.First().Id,
                EntityId = requirementId,
                EntityType = EntityTypeEnum.Requirement
            };

            this.fixture.DataContext.LatestView.Add(details);
            this.fixture.DataContext.SaveChanges();
            this.scenarioContext["RequirementsId"] = requirementsId;
        }

        [Given(@"Company is added to latest views")]
        public void AddCompanyToLatestViews()
        {
            Guid companyId = this.scenarioContext.Get<Company>("Company").Id;
            List<Guid> companiesId = this.scenarioContext.ContainsKey("CompaniesId")
                ? this.scenarioContext.Get<List<Guid>>("CompaniesId")
                : new List<Guid>();

            companiesId.Add(companyId);

            var details = new LatestView
            {
                CreatedDate = this.date,
                UserId = this.fixture.DataContext.Users.First().Id,
                EntityId = companyId,
                EntityType = EntityTypeEnum.Company
            };

            this.fixture.DataContext.LatestView.Add(details);
            this.fixture.DataContext.SaveChanges();
            this.scenarioContext["CompaniesId"] = companiesId;
        }

        [Given(@"Contact is added to latest views")]
        public void AddContactToLatestViews()
        {
            Guid contactId = this.scenarioContext.Get<List<Contact>>("Contacts")[0].Id;
           
            var details = new LatestView
            {
                CreatedDate = this.date,
                UserId = this.fixture.DataContext.Users.First().Id,
                EntityId = contactId,
                EntityType = EntityTypeEnum.Contact
            };

            this.fixture.DataContext.LatestView.Add(details);
            this.fixture.DataContext.SaveChanges();
            this.scenarioContext["ContactId"] = contactId;
        }

        [When(@"User adds (.*) to latest viewed entities using api")]
        public void CreateLatestView(string entity)
        {
            var details = new CreateLatestViewCommand();

            switch (entity.ToLower())
            {
                case "property":
                    details.EntityId = this.scenarioContext.Get<Property>("Property").Id;
                    details.EntityType = EntityTypeEnum.Property;
                    break;
                case "activity":
                    details.EntityId = this.scenarioContext.Get<Activity>("Activity").Id;
                    details.EntityType = EntityTypeEnum.Activity;
                    break;
                case "requirement":
                    details.EntityId = this.scenarioContext.Get<Requirement>("Requirement").Id;
                    details.EntityType = EntityTypeEnum.Requirement;
                    break;
                case "company":
                    details.EntityId = this.scenarioContext.Get<Company>("Company").Id;
                    details.EntityType = EntityTypeEnum.Company;
                    break;
                case "contact":
                    details.EntityId = this.scenarioContext.Get<List<Contact>>("Contacts")[0].Id;
                    details.EntityType = EntityTypeEnum.Contact;
                    break;
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

        [Then(@"Latest viewed details should match Property entities")]
        public void CheckLatestsViewedProperties()
        {
            const string entityTypeCode = nameof(EntityTypeEnum.Property);

            List<Guid> entitiesIds = this.scenarioContext.Get<List<Guid>>("PropertiesIds").ToList();
            entitiesIds.Reverse();
            entitiesIds = entitiesIds.Distinct().Take(this.maxLatestViews).ToList();

            List<Guid> addressesIds =
                entitiesIds.Select(id => this.fixture.DataContext.Properties.Single(p => p.Id.Equals(id)).AddressId).ToList();

            LatestViewQueryResultItem response =
                JsonConvert.DeserializeObject<List<LatestViewQueryResultItem>>(this.scenarioContext.GetResponseContent()).Single();

            response.EntityTypeCode.Should().Be(entityTypeCode);

            List<LatestViewData> data = response.List.ToList();

            List<Address> expectedAddresses =
                addressesIds.Select(id => this.fixture.DataContext.Addresses.Single(a => a.Id.Equals(id))).ToList();
            List<Address> currentAddresses =
                data.Select(d => JsonConvert.DeserializeObject<Address>(d.Data.ToString())).ToList();

            expectedAddresses.ShouldAllBeEquivalentTo(currentAddresses, opt => opt
                .Excluding(a => a.Country));

            List<LatestView> latestViews = this.fixture.DataContext.LatestView.Select(r => r).GroupBy(lv => lv.EntityId)
                                               .Select(gLv => gLv.OrderByDescending(lv => lv.CreatedDate).FirstOrDefault())
                                               .OrderByDescending(lv => lv.CreatedDate).Take(this.maxLatestViews).ToList();

            latestViews.Should().Equal(data, (v1, v2) => v1.CreatedDate.Equals(v2.CreatedDate));
        }

        [Then(@"Latest viewed details should match Activity entities")]
        public void CheckLatestsViewedActivities()
        {
            const string entityTypeCode = nameof(EntityTypeEnum.Activity);

            List<Guid> entitiesIds = this.scenarioContext.Get<List<Guid>>("ActivitiesIds").ToList();
            entitiesIds.Reverse();
            entitiesIds = entitiesIds.Distinct().Take(this.maxLatestViews).ToList();

            List<Guid> addressesIds =
                entitiesIds.Select(id => this.fixture.DataContext.Activities.Single(p => p.Id.Equals(id)).Property.AddressId)
                           .ToList();

            LatestViewQueryResultItem response =
                JsonConvert.DeserializeObject<List<LatestViewQueryResultItem>>(this.scenarioContext.GetResponseContent()).Single();

            response.EntityTypeCode.Should().Be(entityTypeCode);

            List<LatestViewData> data = response.List.ToList();

            List<Address> expectedAddresses =
                addressesIds.Select(id => this.fixture.DataContext.Addresses.Single(a => a.Id.Equals(id))).ToList();
            List<Address> currentAddresses =
                data.Select(d => JsonConvert.DeserializeObject<Address>(d.Data.ToString())).ToList();

            expectedAddresses.ShouldAllBeEquivalentTo(currentAddresses, opt => opt
                .Excluding(a => a.Country));

            List<LatestView> latestViews = this.fixture.DataContext.LatestView.Select(r => r).GroupBy(lv => lv.EntityId)
                                               .Select(gLv => gLv.OrderByDescending(lv => lv.CreatedDate).FirstOrDefault())
                                               .OrderByDescending(lv => lv.CreatedDate).Take(this.maxLatestViews).ToList();

            latestViews.Should().Equal(data, (v1, v2) => v1.CreatedDate.Equals(v2.CreatedDate));
        }

        [Then(@"Latest viewed details should match Requirement entities")]
        public void CheckLatestsViewedRequirements()
        {
            const string entityTypeCode = nameof(EntityTypeEnum.Requirement);

            List<Guid> entitiesIds = this.scenarioContext.Get<List<Guid>>("RequirementsId").ToList();
            entitiesIds.Reverse();
            entitiesIds = entitiesIds.Distinct().Take(this.maxLatestViews).ToList();

            LatestViewQueryResultItem response =
                JsonConvert.DeserializeObject<List<LatestViewQueryResultItem>>(this.scenarioContext.GetResponseContent()).Single();

            response.EntityTypeCode.Should().Be(entityTypeCode);

            List<LatestViewData> data = response.List.ToList();

            List<List<Contact>> currentContacts =
                data.Select(d => JsonConvert.DeserializeObject<List<Contact>>(d.Data.ToString())).ToList();

            List<List<Contact>> expectedContacts =
                entitiesIds.Select(
                    guid => this.fixture.DataContext.Requirements.Single(req => req.Id.Equals(guid)).Contacts.ToList())
                           .ToList();

            expectedContacts.ShouldAllBeEquivalentTo(currentContacts);

            List<LatestView> latestViews = this.fixture.DataContext.LatestView.Select(r => r).GroupBy(lv => lv.EntityId)
                                               .Select(gLv => gLv.OrderByDescending(lv => lv.CreatedDate).FirstOrDefault())
                                               .OrderByDescending(lv => lv.CreatedDate).Take(this.maxLatestViews).ToList();

            latestViews.Should().Equal(data, (v1, v2) => v1.CreatedDate.Equals(v2.CreatedDate));
        }

        [Then(@"Latest viewed details should match Company entities")]
        public void CheckLatestsViewedCompanies()
        {
            const string entityTypeCode = nameof(EntityTypeEnum.Company);

            List<Guid> entitiesIds = this.scenarioContext.Get<List<Guid>>("CompaniesId").ToList();
            entitiesIds.Reverse();
            entitiesIds = entitiesIds.Distinct().Take(this.maxLatestViews).ToList();

            LatestViewQueryResultItem response =
                JsonConvert.DeserializeObject<List<LatestViewQueryResultItem>>(this.scenarioContext.GetResponseContent()).Single();

            response.EntityTypeCode.Should().Be(entityTypeCode);

            List<LatestViewData> data = response.List.ToList();

            List<Company> currentCompanies = data.Select(d => JsonConvert.DeserializeObject<Company>(d.Data.ToString())).ToList();

            List<Company> expectedCompanies =
                entitiesIds.Select(guid => this.fixture.DataContext.Companies.Single(c => c.Id.Equals(guid))).ToList();

            expectedCompanies.ShouldAllBeEquivalentTo(currentCompanies, opt => opt.Excluding(c => c.CompaniesContacts).Excluding(c => c.ClientCareStatus));

            List<LatestView> latestViews = this.fixture.DataContext.LatestView.Select(r => r).GroupBy(lv => lv.EntityId)
                                               .Select(gLv => gLv.OrderByDescending(lv => lv.CreatedDate).FirstOrDefault())
                                               .OrderByDescending(lv => lv.CreatedDate).Take(this.maxLatestViews).ToList();

            latestViews.Should().Equal(data, (v1, v2) => v1.CreatedDate.Equals(v2.CreatedDate));
        }

        [Then(@"Retrieved latest view should contain Property entity")]
        public void CheckLatestsViewedProperty()
        {
            const string entityTypeCode = nameof(EntityTypeEnum.Property);
            Guid entityId = this.scenarioContext.Get<Property>("Property").Id;
            Guid addressId = this.fixture.DataContext.Properties.Single(p => p.Id.Equals(entityId)).AddressId;

            LatestViewQueryResultItem response =
                JsonConvert.DeserializeObject<List<LatestViewQueryResultItem>>(this.scenarioContext.GetResponseContent()).Single();

            response.EntityTypeCode.Should().Be(entityTypeCode);

            List<LatestViewData> data = response.List.ToList();
            data.Should().HaveCount(1);

            var currentAddress = JsonConvert.DeserializeObject<Address>(data.Single().Data.ToString());
            Address expectedAddress = this.fixture.DataContext.Addresses.Single(a => a.Id.Equals(addressId));

            expectedAddress.ShouldBeEquivalentTo(currentAddress, opt => opt.Excluding(a => a.Country));

            LatestView latestView = this.fixture.DataContext.LatestView.Single();

            latestView.CreatedDate.Should().Be(data.Single().CreatedDate);
            latestView.EntityTypeString.Should().Be(entityTypeCode);
            latestView.EntityId.Should().Be(entityId);
            latestView.EntityType.Should().Be(EntityTypeEnum.Property);
        }

        [Then(@"Retrieved latest view should contain Activity entity")]
        public void CheckLatestsViewedActivity()
        {
            const string entityTypeCode = nameof(EntityTypeEnum.Activity);
            Guid entityId = this.scenarioContext.Get<Activity>("Activity").Id;
            Guid addressId = this.fixture.DataContext.Activities.Single(p => p.Id.Equals(entityId)).Property.AddressId;

            LatestViewQueryResultItem response =
                JsonConvert.DeserializeObject<List<LatestViewQueryResultItem>>(this.scenarioContext.GetResponseContent()).Single();

            response.EntityTypeCode.Should().Be(entityTypeCode);

            List<LatestViewData> data = response.List.ToList();
            data.Should().HaveCount(1);

            var currentAddress = JsonConvert.DeserializeObject<Address>(data.Single().Data.ToString());
            Address expectedAddress = this.fixture.DataContext.Addresses.Single(a => a.Id.Equals(addressId));

            expectedAddress.ShouldBeEquivalentTo(currentAddress, opt => opt.Excluding(a => a.Country));

            LatestView latestView = this.fixture.DataContext.LatestView.Single();

            latestView.CreatedDate.Should().Be(data.Single().CreatedDate);
            latestView.EntityTypeString.Should().Be(entityTypeCode);
            latestView.EntityId.Should().Be(entityId);
            latestView.EntityType.Should().Be(EntityTypeEnum.Activity);
        }

        [Then(@"Retrieved latest view should contain Contact entity")]
        public void CheckLatestsViewedContact()
        {
            const string entityTypeCode = nameof(EntityTypeEnum.Contact);
            Guid entityId = this.scenarioContext.Get<List<Contact>>("Contacts")[0].Id;
            

            LatestViewQueryResultItem response =
                JsonConvert.DeserializeObject<List<LatestViewQueryResultItem>>(this.scenarioContext.GetResponseContent()).Single();

            response.EntityTypeCode.Should().Be(entityTypeCode);

            List<LatestViewData> data = response.List.ToList();
            data.Should().HaveCount(1);

            LatestView latestView = this.fixture.DataContext.LatestView.Single();

            latestView.CreatedDate.Should().Be(data.Single().CreatedDate);
            latestView.EntityTypeString.Should().Be(entityTypeCode);
            latestView.EntityId.Should().Be(entityId);
            latestView.EntityType.Should().Be(EntityTypeEnum.Contact);
        }

        [Then(@"Retrieved latest view should contain Requirement entity")]
        public void CheckLatestsViewedRequirement()
        {
            const string entityTypeCode = nameof(EntityTypeEnum.Requirement);
            Guid entityId = this.scenarioContext.Get<Requirement>("Requirement").Id;

            LatestViewQueryResultItem response =
                JsonConvert.DeserializeObject<List<LatestViewQueryResultItem>>(this.scenarioContext.GetResponseContent()).Single();

            response.EntityTypeCode.Should().Be(entityTypeCode);

            List<LatestViewData> data = response.List.ToList();
            data.Should().HaveCount(1);

            var currentContacts = JsonConvert.DeserializeObject<List<Contact>>(data.Single().Data.ToString());
            List<Contact> expectedContacts =
                this.fixture.DataContext.Requirements.Single(req => req.Id.Equals(entityId)).Contacts.ToList();

            expectedContacts.Should().Equal(currentContacts,
                (c1, c2) => c1.FirstName.Equals(c2.FirstName) &&
                            c1.LastName.Equals(c2.LastName) &&
                            c1.Title.Equals(c2.Title) &&
                            c1.Id.Equals(c2.Id));

            LatestView latestView = this.fixture.DataContext.LatestView.Single();

            latestView.CreatedDate.Should().Be(data.Single().CreatedDate);
            latestView.EntityTypeString.Should().Be(entityTypeCode);
            latestView.EntityId.Should().Be(entityId);
            latestView.EntityType.Should().Be(EntityTypeEnum.Requirement);
        }

        [Then(@"Retrieved latest view should contain Company entity")]
        public void CheckLatestsViewedCompany()
        {
            const string entityTypeCode = nameof(EntityTypeEnum.Company);
            Guid entityId = this.scenarioContext.Get<Company>("Company").Id;

            LatestViewQueryResultItem response =
                JsonConvert.DeserializeObject<List<LatestViewQueryResultItem>>(this.scenarioContext.GetResponseContent()).Single();

            response.EntityTypeCode.Should().Be(entityTypeCode);

            List<LatestViewData> data = response.List.ToList();
            data.Should().HaveCount(1);

            var currentCompany = JsonConvert.DeserializeObject<Company>(data.Single().Data.ToString());
            Company expectedCompany = this.fixture.DataContext.Companies.Single(c => c.Id.Equals(entityId));

            expectedCompany.ShouldBeEquivalentTo(currentCompany, opt => opt.Excluding(c => c.CompaniesContacts).Excluding(c => c.ClientCareStatus));

            LatestView latestView = this.fixture.DataContext.LatestView.Single();

            latestView.CreatedDate.Should().Be(data.Single().CreatedDate);
            latestView.EntityTypeString.Should().Be(entityTypeCode);
            latestView.EntityId.Should().Be(entityId);
            latestView.EntityType.Should().Be(EntityTypeEnum.Company);
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
