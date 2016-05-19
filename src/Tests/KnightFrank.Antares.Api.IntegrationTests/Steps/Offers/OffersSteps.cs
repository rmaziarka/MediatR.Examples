namespace KnightFrank.Antares.Api.IntegrationTests.Steps.Offers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    using FluentAssertions;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Dal.Model.Offer;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Domain.Offer.Commands;

    using Newtonsoft.Json;

    using TechTalk.SpecFlow;

    [Binding]
    public class OffersSteps
    {
        private const string ApiUrl = "/api/offers";
        private readonly BaseTestClassFixture fixture;

        private readonly ScenarioContext scenarioContext;

        public OffersSteps(BaseTestClassFixture fixture, ScenarioContext scenarioContext)
        {
            this.fixture = fixture;
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            this.scenarioContext = scenarioContext;
        }

        [When(@"User creates (.*) offer using api")]
        public void CreateOfferUsingApi(string status)
        {
            Guid statusId = this.scenarioContext.Get<Dictionary<string, Guid>>("EnumDictionary")[status];
            Guid activityId = this.scenarioContext.Get<Activity>("Activity").Id;
            Guid requirementId = this.scenarioContext.Get<Requirement>("Requirement").Id;

            var details = new CreateOfferCommand
            {
                ActivityId = activityId,
                RequirementId = requirementId,
                StatusId = statusId,
                Price = 10,
                SpecialConditions = StringExtension.GenerateMaxAlphanumericString(400),
                OfferDate = DateTime.UtcNow,
                CompletionDate = DateTime.UtcNow,
                ExchangeDate = DateTime.UtcNow
            };

            this.CreateOffer(details);
        }

        [When(@"User creates (.*) offer with mandatory fields using api")]
        public void CreateOfferWithMandatoryFieldsUsingApi(string status)
        {
            Guid statusId = this.scenarioContext.Get<Dictionary<string, Guid>>("EnumDictionary")[status];
            Guid activityId = this.scenarioContext.Get<Activity>("Activity").Id;
            Guid requirementId = this.scenarioContext.Get<Requirement>("Requirement").Id;

            var details = new CreateOfferCommand
            {
                ActivityId = activityId,
                RequirementId = requirementId,
                StatusId = statusId,
                Price = 10,
                OfferDate = DateTime.UtcNow
            };

            this.CreateOffer(details);
        }

        [When(@"User creates offer with invalid (.*) using api")]
        public void CreateOfferWithInvalidDataUsingApi(string data)
        {
            Guid statusId = data.Equals("status")
                ? Guid.NewGuid()
                : this.scenarioContext.Get<Dictionary<string, Guid>>("EnumDictionary")["New"];
            Guid activityId = data.Equals("activity") ? Guid.NewGuid() : this.scenarioContext.Get<Activity>("Activity").Id;
            Guid requirementId = data.Equals("requirement")
                ? Guid.NewGuid()
                : this.scenarioContext.Get<Requirement>("Requirement").Id;

            var details = new CreateOfferCommand
            {
                ActivityId = activityId,
                RequirementId = requirementId,
                StatusId = statusId,
                Price = 10,
                SpecialConditions = StringExtension.GenerateMaxAlphanumericString(400),
                OfferDate = DateTime.UtcNow,
                CompletionDate = DateTime.UtcNow,
                ExchangeDate = DateTime.UtcNow
            };

            this.CreateOffer(details);
        }

        [Then(@"Offer details should be the same as already added")]
        public void CheckOfferDetails()
        {
            var offer = JsonConvert.DeserializeObject<Offer>(this.scenarioContext.GetResponseContent());
            Offer expectedOffer = this.fixture.DataContext.Offer.Single(o => o.Id.Equals(offer.Id));

            expectedOffer.ShouldBeEquivalentTo(offer, opt => opt
                .Excluding(o => o.Negotiator)
                .Excluding(o => o.Status)
                .Excluding(o => o.Requirement)
                .Excluding(o => o.Activity));
        }

        private void CreateOffer(CreateOfferCommand command)
        {
            string requestUrl = $"{ApiUrl}";
            HttpResponseMessage response = this.fixture.SendPostRequest(requestUrl, command);
            this.scenarioContext.SetHttpResponseMessage(response);
        }
    }
}
