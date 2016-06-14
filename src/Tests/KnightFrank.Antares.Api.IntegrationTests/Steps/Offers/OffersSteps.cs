namespace KnightFrank.Antares.Api.IntegrationTests.Steps.Offers
{
    using System;
    using System.Linq;
    using System.Net.Http;

    using FluentAssertions;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Dal.Model.Company;
    using KnightFrank.Antares.Dal.Model.Offer;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Domain.Common.Enums;
    using KnightFrank.Antares.Domain.Offer.Commands;

    using Newtonsoft.Json;

    using TechTalk.SpecFlow;

    [Binding]
    public class OffersSteps
    {
        private const string ApiUrl = "/api/offers";
        private readonly BaseTestClassFixture fixture;
        private readonly ScenarioContext scenarioContext;
        private readonly DateTime date = DateTime.UtcNow;

        public OffersSteps(BaseTestClassFixture fixture, ScenarioContext scenarioContext)
        {
            this.fixture = fixture;
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            this.scenarioContext = scenarioContext;
        }

        [Given(@"Offer with (.*) status exists in database")]
        public void CreateOfferUsingInDatabase(string status)
        {
            Guid activityId = this.scenarioContext.Get<Activity>("Activity").Id;
            Guid requirementId = this.scenarioContext.Get<Requirement>("Requirement").Id;
            Guid statusId =
                this.fixture.DataContext.EnumTypeItems.Single(
                    e => e.EnumType.Code.Equals(nameof(OfferStatus)) && e.Code.Equals(status)).Id;

            var offer = new Offer
            {
                ActivityId = activityId,
                CompletionDate = this.date,
                ExchangeDate = this.date,
                NegotiatorId = this.fixture.DataContext.Users.First().Id,
                OfferDate = this.date,
                Price = 1000,
                RequirementId = requirementId,
                SpecialConditions = StringExtension.GenerateMaxAlphanumericString(4000),
                StatusId = statusId,
                CreatedDate = this.date,
                LastModifiedDate = this.date
            };

            if (status.ToLower().Equals(nameof(OfferStatus.Accepted).ToLower()))
            {
                CompanyContact companyContact = this.scenarioContext.Get<Company>("Company").CompaniesContacts.First();

                offer.MortgageStatusId =
                    this.fixture.DataContext.EnumTypeItems.Single(
                        e => e.EnumType.Code.Equals(nameof(MortgageStatus)) && e.Code.Equals(nameof(MortgageStatus.Unknown))).Id;

                offer.MortgageSurveyStatusId =
                    this.fixture.DataContext.EnumTypeItems.Single(
                        e =>
                            e.EnumType.Code.Equals(nameof(MortgageSurveyStatus)) &&
                            e.Code.Equals(nameof(MortgageSurveyStatus.Unknown))).Id;

                offer.AdditionalSurveyStatusId =
                    this.fixture.DataContext.EnumTypeItems.Single(
                        e =>
                            e.EnumType.Code.Equals(nameof(AdditionalSurveyStatus)) &&
                            e.Code.Equals(nameof(AdditionalSurveyStatus.Unknown))).Id;

                offer.SearchStatusId =
                    this.fixture.DataContext.EnumTypeItems.Single(
                        e => e.EnumType.Code.Equals(nameof(SearchStatus)) && e.Code.Equals(nameof(SearchStatus.NotStarted))).Id;

                offer.EnquiriesId =
                    this.fixture.DataContext.EnumTypeItems.Single(
                        e => e.EnumType.Code.Equals(nameof(Enquiries)) && e.Code.Equals(nameof(Enquiries.NotStarted))).Id;

                offer.ContractApproved = true;

                offer.MortgageLoanToValue = 100;

                offer.BrokerId = companyContact.ContactId;
                offer.BrokerCompanyId = companyContact.CompanyId;
                
                offer.LenderId = companyContact.ContactId;
                offer.LenderCompanyId = companyContact.CompanyId;

                offer.MortgageSurveyDate = DateTime.UtcNow;

                offer.SurveyorId = companyContact.ContactId;
                offer.SurveyorCompanyId = companyContact.CompanyId;

                offer.AdditionalSurveyDate = DateTime.UtcNow;

                offer.AdditionalSurveyorId = companyContact.ContactId;
                offer.AdditionalSurveyorCompanyId = companyContact.CompanyId;

                offer.ProgressComment = StringExtension.GenerateMaxAlphanumericString(4000);
            }

            this.fixture.DataContext.Offer.Add(offer);
            this.fixture.DataContext.SaveChanges();

            this.scenarioContext.Set(offer, "Offer");
        }

        [When(@"User creates (.*) offer using api")]
        public void CreateOfferUsingApi(string status)
        {
            Guid statusId =
                this.fixture.DataContext.EnumTypeItems.Single(
                    e => e.EnumType.Code.Equals(nameof(OfferStatus)) && e.Code.Equals(status)).Id;
            Guid activityId = this.scenarioContext.Get<Activity>("Activity").Id;
            Guid requirementId = this.scenarioContext.Get<Requirement>("Requirement").Id;

            var details = new CreateOfferCommand
            {
                ActivityId = activityId,
                RequirementId = requirementId,
                StatusId = statusId,
                Price = 10,
                SpecialConditions = StringExtension.GenerateMaxAlphanumericString(400),
                OfferDate = this.date,
                CompletionDate = this.date,
                ExchangeDate = this.date
            };

            this.CreateOffer(details);
        }

        [When(@"User creates (.*) offer with mandatory fields using api")]
        public void CreateOfferWithMandatoryFieldsUsingApi(string status)
        {
            Guid statusId =
                this.fixture.DataContext.EnumTypeItems.Single(
                    e => e.EnumType.Code.Equals(nameof(OfferStatus)) && e.Code.Equals(status)).Id;
            Guid activityId = this.scenarioContext.Get<Activity>("Activity").Id;
            Guid requirementId = this.scenarioContext.Get<Requirement>("Requirement").Id;

            var details = new CreateOfferCommand
            {
                ActivityId = activityId,
                RequirementId = requirementId,
                StatusId = statusId,
                Price = 10,
                OfferDate = this.date
            };

            this.CreateOffer(details);
        }

        [When(@"User creates offer with invalid (.*) using api")]
        public void CreateOfferWithInvalidDataUsingApi(string data)
        {
            Guid statusId = data.Equals("status")
                ? Guid.NewGuid()
                : this.fixture.DataContext.EnumTypeItems.Single(
                    e => e.EnumType.Code.Equals(nameof(OfferStatus)) && e.Code.Equals(nameof(OfferStatus.New))).Id;
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
                OfferDate = this.date,
                CompletionDate = this.date,
                ExchangeDate = this.date
            };

            this.CreateOffer(details);
        }

        [When(@"User gets offer for (.*) id")]
        public void GetOffer(string id)
        {
            Guid offerId = id.Equals("latest") ? this.scenarioContext.Get<Offer>("Offer").Id : Guid.NewGuid();
            string requestUrl = $"{ApiUrl}/{offerId}";

            HttpResponseMessage response = this.fixture.SendGetRequest(requestUrl);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [When(@"User updates offer with (.*) status")]
        public void UpdateOffer(string status)
        {
            var offer = this.scenarioContext.Get<Offer>("Offer");
            var details = new UpdateOfferCommand
            {
                Id = offer.Id,
                StatusId =
                    this.fixture.DataContext.EnumTypeItems.Single(
                        e => e.EnumType.Code.Equals(nameof(OfferStatus)) && e.Code.Equals(status)).Id,
                Price = 2000,
                SpecialConditions = StringExtension.GenerateMaxAlphanumericString(4000),
                CompletionDate = this.date.AddDays(2),
                ExchangeDate = this.date.AddDays(2),
                OfferDate = this.date.AddMinutes(-1)
            };

            if (status.ToLower().Equals(nameof(OfferStatus.Accepted).ToLower()))
            {
                CompanyContact companyContact = this.scenarioContext.Get<Company>("Company").CompaniesContacts.First();

                details.MortgageStatusId =
                    this.fixture.DataContext.EnumTypeItems.Single(
                        e => e.EnumType.Code.Equals(nameof(MortgageStatus)) && e.Code.Equals(nameof(MortgageStatus.Agreed))).Id;

                details.MortgageSurveyStatusId =
                    this.fixture.DataContext.EnumTypeItems.Single(
                        e =>
                            e.EnumType.Code.Equals(nameof(MortgageSurveyStatus)) &&
                            e.Code.Equals(nameof(MortgageSurveyStatus.Complete))).Id;

                details.AdditionalSurveyStatusId =
                    this.fixture.DataContext.EnumTypeItems.Single(
                        e =>
                            e.EnumType.Code.Equals(nameof(AdditionalSurveyStatus)) &&
                            e.Code.Equals(nameof(AdditionalSurveyStatus.Complete))).Id;

                details.SearchStatusId =
                    this.fixture.DataContext.EnumTypeItems.Single(
                        e => e.EnumType.Code.Equals(nameof(SearchStatus)) && e.Code.Equals(nameof(SearchStatus.Complete))).Id;

                details.EnquiriesId =
                    this.fixture.DataContext.EnumTypeItems.Single(
                        e => e.EnumType.Code.Equals(nameof(Enquiries)) && e.Code.Equals(nameof(Enquiries.Complete))).Id;

                details.ContractApproved = false;

                details.MortgageLoanToValue = 0;

                details.BrokerId = companyContact.ContactId;
                details.BrokerCompanyId = companyContact.CompanyId;

                details.LenderId = companyContact.ContactId;
                details.LenderCompanyId = companyContact.CompanyId;

                details.MortgageSurveyDate = DateTime.UtcNow;

                details.SurveyorId = companyContact.ContactId;
                details.SurveyorCompanyId = companyContact.CompanyId;

                details.AdditionalSurveyDate = DateTime.UtcNow;

                details.AdditionalSurveyorId = companyContact.ContactId;
                details.AdditionalSurveyorCompanyId = companyContact.CompanyId;

                details.ProgressComment = StringExtension.GenerateMaxAlphanumericString(4000);
            }

            this.UpdateOffer(details);
        }

        [When(@"User updates offer with invalid (.*) data")]
        public void UpdateOfferWithInvalidData(string data)
        {
            var offer = this.scenarioContext.Get<Offer>("Offer");
            var details = new UpdateOfferCommand
            {
                Id = data.Equals("offer") ? Guid.NewGuid() : offer.Id,
                StatusId =
                    data.Equals("status")
                        ? Guid.NewGuid()
                        : this.fixture.DataContext.EnumTypeItems.Single(
                            e => e.EnumType.Code.Equals(nameof(OfferStatus)) && e.Code.Equals(nameof(OfferStatus.New))).Id,
                CompletionDate = this.date,
                ExchangeDate = this.date,
                SpecialConditions = StringExtension.GenerateMaxAlphanumericString(4000),
                Price = 1000,
                OfferDate = this.date.AddMinutes(-1)
            };

            this.UpdateOffer(details);
        }

        [Then(@"Offer details should be the same as already added")]
        [Then(@"Offer details should be updated")]
        public void CheckOfferDetails()
        {
            var offer = JsonConvert.DeserializeObject<Offer>(this.scenarioContext.GetResponseContent());
            Offer expectedOffer = this.fixture.DataContext.Offer.Single(o => o.Id.Equals(offer.Id));

            expectedOffer.ShouldBeEquivalentTo(offer, opt => opt
                .Excluding(o => o.Negotiator)
                .Excluding(o => o.Status)
                .Excluding(o => o.Requirement)
                .Excluding(o => o.Activity)
                .Excluding(o => o.MortgageStatus)
                .Excluding(o => o.SearchStatus)
                .Excluding(o => o.MortgageSurveyStatus)
                .Excluding(o => o.AdditionalSurveyStatus)
                .Excluding(o => o.Enquiries)
                .Excluding(o => o.Broker)
                .Excluding(o => o.BrokerCompany)
                .Excluding(o => o.Lender)
                .Excluding(o => o.LenderCompany)
                .Excluding(o => o.Surveyor)
                .Excluding(o => o.SurveyorCompany)
                .Excluding(o => o.AdditionalSurveyor)
                .Excluding(o => o.AdditionalSurveyorCompany));

            expectedOffer.NegotiatorId.Should().NotBeEmpty();
        }

        [Then(@"Offer details in requirement should be the same as added")]
        public void CompareRequirementOffers()
        {
            Offer offer = JsonConvert.DeserializeObject<Requirement>(this.scenarioContext.GetResponseContent()).Offers.Single();
            Offer expectedOffer = this.fixture.DataContext.Offer.Single(o => o.Id.Equals(offer.Id));

            offer.ShouldBeEquivalentTo(expectedOffer,
                opt => opt.Excluding(o => o.Activity)
                          .Excluding(o => o.Negotiator)
                          .Excluding(o => o.Requirement)
                          .Excluding(o => o.Status));
        }

        private void CreateOffer(CreateOfferCommand command)
        {
            string requestUrl = $"{ApiUrl}";
            HttpResponseMessage response = this.fixture.SendPostRequest(requestUrl, command);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        private void UpdateOffer(UpdateOfferCommand command)
        {
            string requestUrl = $"{ApiUrl}";
            HttpResponseMessage response = this.fixture.SendPutRequest(requestUrl, command);
            this.scenarioContext.SetHttpResponseMessage(response);
        }
    }
}
