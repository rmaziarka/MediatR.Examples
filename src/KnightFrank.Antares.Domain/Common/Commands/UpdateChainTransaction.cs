namespace KnightFrank.Antares.Domain.Common.Commands
{
    using System;

    public class UpdateChainTransaction
    {
        public Guid Id { get; set; }

        public Guid? ActivityId { get; set; }

        public Guid? RequirementId { get; set; }

        public Guid? ParentId { get; set; }

        public bool IsEnd { get; set; }

        public Guid PropertyId { get; set; }

        public string Vendor { get; set; }

        public Guid? AgentUserId { get; set; }

        public Guid? AgentContactId { get; set; }

        public Guid? AgentCompanyId { get; set; }

        public Guid? SolicitorContactId { get; set; }

        public Guid? SolicitorCompanyId { get; set; }

        public Guid MortgageId { get; set; }

        public Guid SurveyId { get; set; }

        public Guid SearchesId { get; set; }

        public Guid EnquiriesId { get; set; }

        public Guid ContractAgreedId { get; set; }

        public DateTime? SurveyDate { get; set; }
    }
}
