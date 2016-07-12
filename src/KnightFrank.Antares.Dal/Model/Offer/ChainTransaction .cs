namespace KnightFrank.Antares.Dal.Model.Offer
{
    using System;

    using Common;
    using Enum;
    using Contacts;

    using KnightFrank.Antares.Dal.Model.Company;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Model.User;
    using Property;

    public class ChainTransaction : BaseEntity, IAuditableEntity
    {
        public Guid? ActivityId { get; set; }

        public virtual Activity Activity { get; set; }

        public Guid? RequirementId { get; set; }

        public virtual Requirement Requirement { get; set; }

        public Guid? ParentId { get; set; }

        public ChainTransaction Parent { get; set; }

        public bool IsEnd { get; set; }

        public Guid PropertyId { get; set; }

        public virtual Property Property { get; set; }

        public string Vendor { get; set; }

        public Guid? AgentUserId { get; set; }

        public virtual User AgentUser { get; set; }

        public Guid? AgentContactId { get; set; }

        public virtual Contact AgentContact { get; set; }

        public Guid? AgentCompanyId { get; set; }

        public virtual Company AgentCompany { get; set; }

        public Guid? SolicitorContactId { get; set; }

        public virtual Contact SolicitorContact { get; set; }

        public Guid? SolicitorCompanyId { get; set; }

        public virtual Company SolicitorCompany { get; set; }

        public Guid MortgageId { get; set; }

        public virtual EnumTypeItem Mortgage { get; set; }

        public Guid SurveyId { get; set; }

        public virtual EnumTypeItem Survey { get; set; }

        public Guid SearchesId { get; set; }

        public virtual EnumTypeItem Searches { get; set; }

        public Guid EnquiriesId { get; set; }

        public virtual EnumTypeItem Enquiries { get; set; }

        public Guid ContractAgreedId { get; set; }

        public virtual EnumTypeItem ContractAgreed { get; set; }

        public DateTime? SurveyDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public bool IsKnightFrankAgent { get; set; }
    }
}
