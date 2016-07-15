namespace KnightFrank.Antares.UITests.Steps.Database
{
    using System;
    using System.Linq;

    using KnightFrank.Antares.Dal;
    using KnightFrank.Antares.Dal.Model.Company;
    using KnightFrank.Antares.Dal.Model.Offer;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Domain.Common.Enums;
    using KnightFrank.Antares.UITests.Pages.Panels;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class ChainTransactionSteps
    {
        private readonly KnightFrankContext dataContext;
        private readonly ScenarioContext scenarioContext;

        public ChainTransactionSteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            this.scenarioContext = scenarioContext;
            this.dataContext = this.scenarioContext.Get<KnightFrankContext>("DataContext");
        }

        [Given(@"Upward chain is created in database")]
        public void CreateUpwardChainInDb(Table table)
        {
            var details = table.CreateInstance<ChainTransactionData>();

            var activity = this.scenarioContext.Get<Activity>("Activity");
            var property = this.scenarioContext.Get<Property>("Property");
            var requirement = this.scenarioContext.Get<Requirement>("Requirement");
            var company = this.scenarioContext.Get<Company>("Company");

            var upwardChain = new ChainTransaction
            {
                ActivityId = activity.Id,
                PropertyId = property.Id,
                RequirementId = requirement.Id,
                IsEnd = bool.Parse(details.EndOfChain),
                Vendor = details.Vendor,
                SolicitorCompanyId = company.CompaniesContacts.Last().CompanyId,
                SolicitorContactId = company.CompaniesContacts.Last().ContactId,
                MortgageId =
                    this.dataContext.EnumTypeItems.Single(
                        e =>
                            e.EnumType.Code.Equals(nameof(ChainMortgageStatus)) &&
                            e.Code.Equals(nameof(ChainMortgageStatus.Unknown))).Id,
                SurveyId =
                    this.dataContext.EnumTypeItems.Single(
                        e =>
                            e.EnumType.Code.Equals(nameof(ChainMortgageSurveyStatus)) &&
                            e.Code.Equals(nameof(ChainMortgageSurveyStatus.Unknown))).Id,
                SurveyDate = DateTime.UtcNow,
                SearchesId =
                    this.dataContext.EnumTypeItems.Single(
                        e =>
                            e.EnumType.Code.Equals(nameof(ChainSearchStatus)) &&
                            e.Code.Equals(nameof(ChainSearchStatus.Outstanding))).Id,
                EnquiriesId =
                    this.dataContext.EnumTypeItems.Single(
                        e => e.EnumType.Code.Equals(nameof(ChainEnquiries)) && e.Code.Equals(nameof(ChainEnquiries.Outstanding)))
                        .Id,
                ContractAgreedId =
                    this.dataContext.EnumTypeItems.Single(
                        e =>
                            e.EnumType.Code.Equals(nameof(ChainContractAgreedStatus)) &&
                            e.Code.Equals(nameof(ChainContractAgreedStatus.Outstanding))).Id,
                ParentId =
                    this.scenarioContext.ContainsKey("ChainTransaction")
                        ? this.scenarioContext.Get<ChainTransaction>("ChainTransaction").Id
                        : (Guid?)null,
                CreatedDate = DateTime.UtcNow,
                LastModifiedDate = DateTime.UtcNow
            };

            if (bool.Parse(details.KnightFrankAgent))
            {
                upwardChain.AgentCompanyId = null;
                upwardChain.AgentContactId = null;
                upwardChain.AgentUserId = this.dataContext.Users.First().Id;
            }
            else
            {
                upwardChain.AgentCompanyId = company.CompaniesContacts.First().CompanyId;
                upwardChain.AgentContactId = company.CompaniesContacts.First().ContactId;
                upwardChain.AgentUserId = null;
            }

            activity.ChainTransactions.Add(upwardChain);
            this.dataContext.SaveChanges();

            this.scenarioContext.Set(activity, "Activity");
            this.scenarioContext["ChainTransaction"] = upwardChain;
        }

        [Given(@"Downward chain is created in database")]
        public void CreateDownwardChainInDb(Table table)
        {
        }
    }
}
