namespace KnightFrank.Antares.Api.IntegrationTests.Steps.Activities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    using FluentAssertions;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Dal.Model.Company;
    using KnightFrank.Antares.Dal.Model.Offer;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.Common.Enums;

    using Newtonsoft.Json;

    using TechTalk.SpecFlow;

    [Binding]
    public class UpwardChainsSteps
    {
        private const string ApiUrl = "/api/activities";
        private readonly DateTime date = DateTime.UtcNow;
        private readonly BaseTestClassFixture fixture;
        private readonly ScenarioContext scenarioContext;

        public UpwardChainsSteps(BaseTestClassFixture fixture, ScenarioContext scenarioContext)
        {
            this.fixture = fixture;
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            this.scenarioContext = scenarioContext;
        }

        [Given(@"Upward chain exists in database")]
        public void CreateUpwardChainInDatabase()
        {
            var activity = this.scenarioContext.Get<Activity>("Activity");
            var property = this.scenarioContext.Get<Property>("Property");
            var requirement = this.scenarioContext.Get<Requirement>("Requirement");
            var company = this.scenarioContext.Get<Company>("Company");

            var chainTransaction = new ChainTransaction
            {
                ActivityId = activity.Id,
                RequirementId = requirement.Id,
                PropertyId = property.Id,
                IsEnd = true,
                Vendor = StringExtension.GenerateMaxAlphanumericString(128),
                AgentCompanyId = company.CompaniesContacts.First().CompanyId,
                AgentContactId = company.CompaniesContacts.First().ContactId,
                AgentUserId = null,
                SolicitorCompanyId = company.CompaniesContacts.First().CompanyId,
                SolicitorContactId = company.CompaniesContacts.First().ContactId,
                CreatedDate = this.date,
                LastModifiedDate = this.date,
                SurveyDate = this.date,
                MortgageId =
                    this.fixture.DataContext.EnumTypeItems.Single(
                        e =>
                            e.EnumType.Code.Equals(nameof(ChainMortgageStatus)) &&
                            e.Code.Equals(nameof(ChainMortgageStatus.Unknown))).Id,
                SurveyId =
                    this.fixture.DataContext.EnumTypeItems.Single(
                        e =>
                            e.EnumType.Code.Equals(nameof(ChainMortgageSurveyStatus)) &&
                            e.Code.Equals(nameof(ChainMortgageSurveyStatus.Unknown))).Id,
                SearchesId =
                    this.fixture.DataContext.EnumTypeItems.Single(
                        e =>
                            e.EnumType.Code.Equals(nameof(ChainSearchStatus)) &&
                            e.Code.Equals(nameof(ChainSearchStatus.Outstanding))).Id,
                EnquiriesId =
                    this.fixture.DataContext.EnumTypeItems.Single(
                        e => e.EnumType.Code.Equals(nameof(ChainEnquiries)) && e.Code.Equals(nameof(ChainEnquiries.Outstanding)))
                        .Id,
                ContractAgreedId =
                    this.fixture.DataContext.EnumTypeItems.Single(
                        e =>
                            e.EnumType.Code.Equals(nameof(ChainContractAgreedStatus)) &&
                            e.Code.Equals(nameof(ChainContractAgreedStatus.Outstanding))).Id,
                ParentId = null
            };

            activity.ChainTransactions.Add(chainTransaction);

            this.fixture.DataContext.SaveChanges();

            this.scenarioContext.Set(activity, "Activity");
        }

        [When(@"User updates activity with upward chain")]
        public void UpdateActivityWithChainTransaction()
        {
            var activity = this.scenarioContext.Get<Activity>("Activity");
            var requirement = this.scenarioContext.Get<Requirement>("Requirement");
            var property = this.scenarioContext.Get<Property>("Property");
            var company = this.scenarioContext.Get<Company>("Company");

            var chainTransaction = new ChainTransaction
            {
                ActivityId = activity.Id,
                RequirementId = requirement.Id,
                PropertyId = property.Id,
                IsEnd = true,
                Vendor = StringExtension.GenerateMaxAlphanumericString(128),
                AgentCompanyId = company.CompaniesContacts.First().CompanyId,
                AgentContactId = company.CompaniesContacts.First().ContactId,
                AgentUserId = null,
                SolicitorCompanyId = company.CompaniesContacts.First().CompanyId,
                SolicitorContactId = company.CompaniesContacts.First().ContactId,
                CreatedDate = this.date,
                LastModifiedDate = this.date,
                SurveyDate = this.date,
                MortgageId =
                    this.fixture.DataContext.EnumTypeItems.Single(
                        e =>
                            e.EnumType.Code.Equals(nameof(ChainMortgageStatus)) &&
                            e.Code.Equals(nameof(ChainMortgageStatus.Unknown))).Id,
                SurveyId =
                    this.fixture.DataContext.EnumTypeItems.Single(
                        e =>
                            e.EnumType.Code.Equals(nameof(ChainMortgageSurveyStatus)) &&
                            e.Code.Equals(nameof(ChainMortgageSurveyStatus.Unknown))).Id,
                SearchesId =
                    this.fixture.DataContext.EnumTypeItems.Single(
                        e =>
                            e.EnumType.Code.Equals(nameof(ChainSearchStatus)) &&
                            e.Code.Equals(nameof(ChainSearchStatus.Outstanding))).Id,
                EnquiriesId =
                    this.fixture.DataContext.EnumTypeItems.Single(
                        e => e.EnumType.Code.Equals(nameof(ChainEnquiries)) && e.Code.Equals(nameof(ChainEnquiries.Outstanding)))
                        .Id,
                ContractAgreedId =
                    this.fixture.DataContext.EnumTypeItems.Single(
                        e =>
                            e.EnumType.Code.Equals(nameof(ChainContractAgreedStatus)) &&
                            e.Code.Equals(nameof(ChainContractAgreedStatus.Outstanding))).Id,
                ParentId = null
            };

            var updateActivityCommand = new UpdateActivityCommand
            {
                Id = activity.Id,
                ActivityStatusId = activity.ActivityStatusId,
                ActivityTypeId = activity.ActivityTypeId,
                SourceId = activity.SourceId,
                SellingReasonId = activity.SellingReasonId,
                AppraisalMeetingStart = this.date.AddHours(24),
                AppraisalMeetingEnd = this.date.AddHours(26),
                Departments =
                    new List<UpdateActivityDepartment>
                    {
                        new UpdateActivityDepartment
                        {
                            DepartmentId = activity.ActivityDepartments.First().DepartmentId,
                            DepartmentTypeId =
                                this.fixture.DataContext.EnumTypeItems.Single(
                                    i =>
                                        i.EnumType.Code.Equals(nameof(ActivityDepartmentType)) &&
                                        i.Code.Equals(nameof(ActivityDepartmentType.Managing))).Id
                        }
                    },
                LeadNegotiator =
                    new UpdateActivityUser
                    {
                        UserId = activity.ActivityUsers.First().UserId,
                        CallDate = this.date.AddDays(1)
                    },
                ChainTransactions = new List<ChainTransaction>
                {
                    chainTransaction
                }
            };

            this.UpdateActivity(updateActivityCommand);
        }

        [When(@"User updates activity with upward chain with mandatory fields")]
        public void UpdateActivityWithChainTransactionMandatoryFields()
        {
            var activity = this.scenarioContext.Get<Activity>("Activity");
            var requirement = this.scenarioContext.Get<Requirement>("Requirement");
            var property = this.scenarioContext.Get<Property>("Property");
            var company = this.scenarioContext.Get<Company>("Company");

            var chainTransaction = new ChainTransaction
            {
                ActivityId = activity.Id,
                RequirementId = requirement.Id,
                PropertyId = property.Id,
                IsEnd = false,
                Vendor = StringExtension.GenerateMaxAlphanumericString(128),
                AgentCompanyId = null,
                AgentContactId = null,
                AgentUserId = this.fixture.DataContext.Users.First().Id,
                SolicitorCompanyId = company.CompaniesContacts.First().CompanyId,
                SolicitorContactId = company.CompaniesContacts.First().ContactId,
                CreatedDate = this.date,
                LastModifiedDate = this.date,
                MortgageId =
                    this.fixture.DataContext.EnumTypeItems.Single(
                        e =>
                            e.EnumType.Code.Equals(nameof(ChainMortgageStatus)) &&
                            e.Code.Equals(nameof(ChainMortgageStatus.Unknown))).Id,
                SurveyId =
                    this.fixture.DataContext.EnumTypeItems.Single(
                        e =>
                            e.EnumType.Code.Equals(nameof(ChainMortgageSurveyStatus)) &&
                            e.Code.Equals(nameof(ChainMortgageSurveyStatus.Unknown))).Id,
                SearchesId =
                    this.fixture.DataContext.EnumTypeItems.Single(
                        e =>
                            e.EnumType.Code.Equals(nameof(ChainSearchStatus)) &&
                            e.Code.Equals(nameof(ChainSearchStatus.Outstanding))).Id,
                EnquiriesId =
                    this.fixture.DataContext.EnumTypeItems.Single(
                        e => e.EnumType.Code.Equals(nameof(ChainEnquiries)) && e.Code.Equals(nameof(ChainEnquiries.Outstanding)))
                        .Id,
                ContractAgreedId =
                    this.fixture.DataContext.EnumTypeItems.Single(
                        e =>
                            e.EnumType.Code.Equals(nameof(ChainContractAgreedStatus)) &&
                            e.Code.Equals(nameof(ChainContractAgreedStatus.Outstanding))).Id,
                ParentId = null
            };

            var updateActivityCommand = new UpdateActivityCommand
            {
                Id = activity.Id,
                ActivityStatusId = activity.ActivityStatusId,
                ActivityTypeId = activity.ActivityTypeId,
                SourceId = activity.SourceId,
                SellingReasonId = activity.SellingReasonId,
                AppraisalMeetingStart = this.date.AddHours(24),
                AppraisalMeetingEnd = this.date.AddHours(26),
                Departments =
                    new List<UpdateActivityDepartment>
                    {
                        new UpdateActivityDepartment
                        {
                            DepartmentId = activity.ActivityDepartments.First().DepartmentId,
                            DepartmentTypeId =
                                this.fixture.DataContext.EnumTypeItems.Single(
                                    i =>
                                        i.EnumType.Code.Equals(nameof(ActivityDepartmentType)) &&
                                        i.Code.Equals(nameof(ActivityDepartmentType.Managing))).Id
                        }
                    },
                LeadNegotiator =
                    new UpdateActivityUser
                    {
                        UserId = activity.ActivityUsers.First().UserId,
                        CallDate = this.date.AddDays(1)
                    },
                ChainTransactions = new List<ChainTransaction>
                {
                    chainTransaction
                }
            };

            this.UpdateActivity(updateActivityCommand);
        }

        [When(@"User updates activity with upward chain with invalid (.*) data")]
        public void UpdateActivityWithChainTransactionInvalidData(string data)
        {
            var activity = this.scenarioContext.Get<Activity>("Activity");
            var requirement = this.scenarioContext.Get<Requirement>("Requirement");
            var property = this.scenarioContext.Get<Property>("Property");
            var company = this.scenarioContext.Get<Company>("Company");

            var chainTransaction = new ChainTransaction
            {
                ActivityId = data.Equals("ActivityId") ? Guid.NewGuid() : activity.Id,
                RequirementId = data.Equals("RequirementId") ? Guid.NewGuid() : requirement.Id,
                PropertyId = data.Equals("PropertyId") ? Guid.NewGuid() : property.Id,
                IsEnd = false,
                Vendor = StringExtension.GenerateMaxAlphanumericString(128),
                SolicitorCompanyId =
                    data.Equals("SolicitorCompanyId") ? Guid.NewGuid() : company.CompaniesContacts.First().CompanyId,
                SolicitorContactId =
                    data.Equals("SolicitorContactId") ? Guid.NewGuid() : company.CompaniesContacts.First().ContactId,
                CreatedDate = this.date,
                LastModifiedDate = this.date,
                MortgageId =
                    data.Equals("MortgageId")
                        ? Guid.NewGuid()
                        : this.fixture.DataContext.EnumTypeItems.Single(e => e.EnumType.Code.Equals(nameof(ChainMortgageStatus)) &&
                                                                             e.Code.Equals(nameof(ChainMortgageStatus.Unknown))).Id,
                SurveyId = data.Equals("SurveyId")
                    ? Guid.NewGuid()
                    : this.fixture.DataContext.EnumTypeItems.Single(
                        e =>
                            e.EnumType.Code.Equals(nameof(ChainMortgageSurveyStatus)) &&
                            e.Code.Equals(nameof(ChainMortgageSurveyStatus.Unknown))).Id,
                SearchesId = data.Equals("SearchesId")
                    ? Guid.NewGuid()
                    : this.fixture.DataContext.EnumTypeItems.Single(
                        e =>
                            e.EnumType.Code.Equals(nameof(ChainSearchStatus)) &&
                            e.Code.Equals(nameof(ChainSearchStatus.Outstanding))).Id,
                EnquiriesId = data.Equals("EnquiriesId")
                    ? Guid.NewGuid()
                    : this.fixture.DataContext.EnumTypeItems.Single(
                        e => e.EnumType.Code.Equals(nameof(ChainEnquiries)) && e.Code.Equals(nameof(ChainEnquiries.Outstanding)))
                          .Id,
                ContractAgreedId = data.Equals("ContractAgreedId")
                    ? Guid.NewGuid()
                    : this.fixture.DataContext.EnumTypeItems.Single(
                        e =>
                            e.EnumType.Code.Equals(nameof(ChainContractAgreedStatus)) &&
                            e.Code.Equals(nameof(ChainContractAgreedStatus.Outstanding))).Id,
                ParentId = data.Equals("ParentId") ? Guid.NewGuid() : (Guid?)null
            };

            switch (data)
            {
                case "AgentUserId":
                    chainTransaction.AgentCompanyId = null;
                    chainTransaction.AgentContactId = null;
                    chainTransaction.AgentUserId = Guid.NewGuid();
                    break;
                case "AgentCompanyId":
                    chainTransaction.AgentCompanyId = Guid.NewGuid();
                    chainTransaction.AgentContactId = company.CompaniesContacts.First().ContactId;
                    chainTransaction.AgentUserId = null;
                    break;
                case "AgentContactId":
                    chainTransaction.AgentCompanyId = company.CompaniesContacts.First().CompanyId;
                    chainTransaction.AgentContactId = Guid.NewGuid();
                    chainTransaction.AgentUserId = null;
                    break;
                default:
                    chainTransaction.AgentCompanyId = null;
                    chainTransaction.AgentContactId = null;
                    chainTransaction.AgentUserId = this.fixture.DataContext.Users.First().Id;
                    break;
            }

            var updateActivityCommand = new UpdateActivityCommand
            {
                Id = activity.Id,
                ActivityStatusId = activity.ActivityStatusId,
                ActivityTypeId = activity.ActivityTypeId,
                SourceId = activity.SourceId,
                SellingReasonId = activity.SellingReasonId,
                AppraisalMeetingStart = this.date.AddHours(24),
                AppraisalMeetingEnd = this.date.AddHours(26),
                Departments =
                    new List<UpdateActivityDepartment>
                    {
                        new UpdateActivityDepartment
                        {
                            DepartmentId = activity.ActivityDepartments.First().DepartmentId,
                            DepartmentTypeId =
                                this.fixture.DataContext.EnumTypeItems.Single(
                                    i =>
                                        i.EnumType.Code.Equals(nameof(ActivityDepartmentType)) &&
                                        i.Code.Equals(nameof(ActivityDepartmentType.Managing))).Id
                        }
                    },
                LeadNegotiator =
                    new UpdateActivityUser
                    {
                        UserId = activity.ActivityUsers.First().UserId,
                        CallDate = this.date.AddDays(1)
                    },
                ChainTransactions = new List<ChainTransaction>
                {
                    chainTransaction
                }
            };

            this.UpdateActivity(updateActivityCommand);
        }

        [Then(@"Upward chain transaction from offer activity should match transaction already added")]
        public void CheckChainTransaction()
        {
            ChainTransaction expected =
                JsonConvert.DeserializeObject<Offer>(this.scenarioContext.GetResponseContent())
                           .Activity.ChainTransactions.Single();
            ChainTransaction current = this.fixture.DataContext.ChainTransactions.Single(ct => ct.Id.Equals(expected.Id));

            expected.ShouldBeEquivalentTo(current,
                opt =>
                    opt.Excluding(ct => ct.Activity)
                       .Excluding(ct => ct.AgentCompany)
                       .Excluding(ct => ct.AgentContact)
                       .Excluding(ct => ct.AgentUser)
                       .Excluding(ct => ct.ContractAgreed)
                       .Excluding(ct => ct.Enquiries)
                       .Excluding(ct => ct.Mortgage)
                       .Excluding(ct => ct.Parent)
                       .Excluding(ct => ct.Property)
                       .Excluding(ct => ct.Requirement)
                       .Excluding(ct => ct.Searches)
                       .Excluding(ct => ct.SolicitorCompany)
                       .Excluding(ct => ct.SolicitorContact)
                       .Excluding(ct => ct.Survey));
        }

        private void UpdateActivity(UpdateActivityCommand updateActivityCommand)
        {
            string requestUrl = $"{ApiUrl}";

            HttpResponseMessage response = this.fixture.SendPutRequest(requestUrl, updateActivityCommand);
            this.scenarioContext.SetHttpResponseMessage(response);
        }
    }
}
