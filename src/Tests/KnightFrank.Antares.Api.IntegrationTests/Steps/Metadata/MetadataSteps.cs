namespace KnightFrank.Antares.Api.IntegrationTests.Steps.Metadata
{
    using System;
    using System.Linq;
    using System.Net.Http;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.Common.Enums;

    using TechTalk.SpecFlow;

    using Xunit;

    using RequirementType = KnightFrank.Antares.Domain.Common.Enums.RequirementType;
    using PropertyType = KnightFrank.Antares.Domain.Common.Enums.PropertyType;

    [Binding]
    public class MetadataSteps : IClassFixture<BaseTestClassFixture>
    {
        private const string ApiUrl = "/api/metadata/{0}";
        private const string AttributesActivity = "attributes/activity?activityTypeId={0}&pageType={1}&propertyTypeId={2}";
        private const string AttributesOffer = "attributes/offer?requirementTypeId={0}&pageType={1}&offerTypeId={2}";
        private const string AttributesRequirement = "attributes/requirement?requirementTypeId={0}&pageType={1}";
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

        [When(@"User gets activity attributes")]
        public void GetActivityAttributes()
        {
            const string pageType = nameof(PageType.Create);
            Dal.Model.Property.PropertyType propertyType =
                this.fixture.DataContext.PropertyTypes.Single(o => o.EnumCode.Equals(nameof(PropertyType.Flat)));
            Dal.Model.Property.Activities.ActivityType activityType =
                this.fixture.DataContext.ActivityTypes.Single(o => o.EnumCode.Equals(nameof(ActivityType.FreeholdSale)));

            this.GetActivityAttributes(pageType, propertyType.Id.ToString(), activityType.Id.ToString());
        }

        [When(@"User gets activity attributes with invalid (.*) data")]
        public void GetActivityAttributesInvalidData(string data)
        {
            string pageType = data.Equals("pageType") ? "invalid" : nameof(PageType.Create);
            Dal.Model.Property.PropertyType propertyType =
                this.fixture.DataContext.PropertyTypes.Single(o => o.EnumCode.Equals(nameof(PropertyType.Flat)));
            Dal.Model.Property.Activities.ActivityType activityType =
                this.fixture.DataContext.ActivityTypes.Single(o => o.EnumCode.Equals(nameof(ActivityType.FreeholdSale)));

            Guid propertyTypeId = data.Equals("propertyType")
                ? Guid.NewGuid()
                : propertyType.Id;

            Guid activityTypeId = data.Equals("activityType")
                ? Guid.NewGuid()
                : activityType.Id;

            this.GetActivityAttributes(pageType, propertyTypeId.ToString(), activityTypeId.ToString());
        }

        [When(@"User gets activity attributes with empty (.*) data")]
        public void GetActivityAttributesEmptyData(string data)
        {
            const string pageType = nameof(PageType.Preview);
            Dal.Model.Property.PropertyType propertyType =
                this.fixture.DataContext.PropertyTypes.Single(o => o.EnumCode.Equals(nameof(PropertyType.Flat)));
            Dal.Model.Property.Activities.ActivityType activityType =
                this.fixture.DataContext.ActivityTypes.Single(o => o.EnumCode.Equals(nameof(ActivityType.FreeholdSale)));

            if (data.Equals("activityType"))
            {
                this.GetActivityAttributes(pageType, propertyType.Id.ToString(), string.Empty);
            }
            else if (data.Equals("propertyType"))
            {
                this.GetActivityAttributes(pageType, string.Empty, activityType.Id.ToString());
            }
            else if (data.Equals("pageType"))
            {
                this.GetActivityAttributes(string.Empty, propertyType.Id.ToString(), activityType.Id.ToString());
            }
        }

        [When(@"User gets offer attributes")]
        public void GetOfferAttributes()
        {
            const string pageType = nameof(PageType.Create);
            Dal.Model.Property.RequirementType requirementType =
                this.fixture.DataContext.RequirementTypes.Single(o => o.EnumCode.Equals(nameof(RequirementType.ResidentialSale)));

            Dal.Model.Offer.OfferType offerType =
                this.fixture.DataContext.OfferTypes.Single(o => o.EnumCode.Equals(nameof(OfferType.ResidentialSale)));

            this.GetOfferAttributes(pageType, requirementType.Id.ToString(), offerType.Id.ToString());
        }

        [When(@"User gets offer attributes with invalid (.*) data")]
        public void GetOfferAttributesInvalidData(string data)
        {
            string pageType = data.Equals("pageType") ? "invalid" : nameof(PageType.Create);
            Dal.Model.Property.RequirementType requirementType =
                this.fixture.DataContext.RequirementTypes.Single(o => o.EnumCode.Equals(nameof(RequirementType.ResidentialSale)));

            Guid requirementTypeId = data.Equals("requirementType")
                ? Guid.NewGuid()
                : requirementType.Id;

            Dal.Model.Offer.OfferType offerType =
                this.fixture.DataContext.OfferTypes.Single(o => o.EnumCode.Equals(nameof(OfferType.ResidentialSale)));

            Guid offerTypeId = data.Equals("offerType")
                ? Guid.NewGuid()
                : offerType.Id;

            this.GetOfferAttributes(pageType, requirementTypeId.ToString(), offerTypeId.ToString());
        }

        [When(@"User gets offer attributes with empty (.*) data")]
        public void GetOfferAttributesEmptyData(string data)
        {
            const string pageType = nameof(PageType.Preview);
            Dal.Model.Property.RequirementType requirementType =
                this.fixture.DataContext.RequirementTypes.Single(o => o.EnumCode.Equals(nameof(RequirementType.ResidentialSale)));
            Dal.Model.Offer.OfferType offerType =
                this.fixture.DataContext.OfferTypes.Single(o => o.EnumCode.Equals(nameof(OfferType.ResidentialSale)));

            if (data.Equals("offerType"))
            {
                this.GetOfferAttributes(pageType, requirementType.Id.ToString(), string.Empty);
            }
            else if (data.Equals("requirementType"))
            {
                this.GetActivityAttributes(pageType, string.Empty, offerType.Id.ToString());
            }
            else if (data.Equals("pageType"))
            {
                this.GetActivityAttributes(string.Empty, requirementType.Id.ToString(), offerType.Id.ToString());
            }
        }

        [When(@"User gets requirement attributes")]
        public void GetRequirementAttributes()
        {
            const string pageType = nameof(PageType.Create);
            Dal.Model.Property.RequirementType requirementType =
                this.fixture.DataContext.RequirementTypes.Single(o => o.EnumCode.Equals(nameof(RequirementType.ResidentialSale)));

            this.GetRequirementAttributes(pageType, requirementType.Id.ToString());
        }

        [When(@"User gets requirement attributes with invalid (.*) data")]
        public void GetRequirementAttributesInvalidData(string data)
        {
            string pageType = data.Equals("pageType") ? "invalid" : nameof(PageType.Create);
            Dal.Model.Property.RequirementType requirementType =
                this.fixture.DataContext.RequirementTypes.Single(o => o.EnumCode.Equals(nameof(RequirementType.ResidentialSale)));

            Guid requirementTypeId = data.Equals("requirementType")
                ? Guid.NewGuid()
                : requirementType.Id;

            this.GetRequirementAttributes(pageType, requirementTypeId.ToString());
        }

        [When(@"User gets requirement attributes with empty (.*) data")]
        public void GetRequirementAttributesEmptyData(string data)
        {
            const string pageType = nameof(PageType.Preview);
            Dal.Model.Property.RequirementType requirementType =
                this.fixture.DataContext.RequirementTypes.Single(o => o.EnumCode.Equals(nameof(RequirementType.ResidentialSale)));

            if (data.Equals("requirementType"))
            {
                this.GetRequirementAttributes(pageType, string.Empty);
            }
            else if (data.Equals("pageType"))
            {
                this.GetRequirementAttributes(string.Empty, requirementType.Id.ToString());
            }
        }

        private void GetActivityAttributes(string pageType, string propertyTypeId, string activityTypeId)
        {
            string attributes = string.Format(AttributesActivity, activityTypeId, pageType, propertyTypeId);
            string requestUrl = string.Format(ApiUrl, attributes);
            HttpResponseMessage response = this.fixture.SendPostRequest(requestUrl, null);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        private void GetOfferAttributes(string pageType, string requirementTypeId, string offerTypeId)
        {
            string attributes = string.Format(AttributesOffer, requirementTypeId, pageType, offerTypeId);
            string requestUrl = string.Format(ApiUrl, attributes);
            HttpResponseMessage response = this.fixture.SendPostRequest(requestUrl, null);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        private void GetRequirementAttributes(string pageType, string requirementTypeId)
        {
            string attributes = string.Format(AttributesRequirement, requirementTypeId, pageType);
            string requestUrl = string.Format(ApiUrl, attributes);
            HttpResponseMessage response = this.fixture.SendPostRequest(requestUrl, null);
            this.scenarioContext.SetHttpResponseMessage(response);
        }
    }
}
