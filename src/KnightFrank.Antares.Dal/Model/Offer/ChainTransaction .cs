namespace KnightFrank.Antares.Dal.Model.Offer
{
    using System;

    using Common;
    using Enum;
    using Contacts;
    using KnightFrank.Antares.Dal.Model.User;
    using Property;

    public class ChainTransaction : BaseEntity, IAuditableEntity
    {
        public Guid? ParentId;

        public ChainTransaction Parent;

        public bool EndOfChain { get; set; }

        public Guid PropertyId { get; set; }

        public virtual Property Property { get; set; }

        public string Vendor { get; set; }

        public Guid? AgentUserId { get; set; }

        public virtual User AgentUser { get; set; }

        public Guid? AgentContactId { get; set; }

        public virtual Contact AgentContact { get; set; }

        public Guid? AgentCompanyId { get; set; }

        public virtual Contact AgentCompany { get; set; }

        public Guid? SolicitorContactId { get; set; }

        public virtual Contact SolicitorContact { get; set; }

        public Guid? SolicitorCompanyId { get; set; }

        public virtual Contact SolicitorCompany { get; set; }

        public Guid? MortgageId { get; set; }

        public EnumTypeItem Mortgage { get; set; }

        public Guid? SurveyId { get; set; }

        public EnumTypeItem Survey { get; set; }

        public Guid? SearchesId { get; set; }

        public EnumTypeItem Searches { get; set; }

        public Guid? EnqueriesId { get; set; }

        public EnumTypeItem Enqueries { get; set; }

        public Guid? ContactAgreedId { get; set; }

        public EnumTypeItem ContactAgreed { get; set; }

        public DateTime? SurveyDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastModifiedDate { get; set; }
    }
}
