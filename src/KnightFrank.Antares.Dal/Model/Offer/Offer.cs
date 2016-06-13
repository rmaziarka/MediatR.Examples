namespace KnightFrank.Antares.Dal.Model.Offer
{
    using System;

    using Common;
    using Enum;

    using Contacts;

    using KnightFrank.Antares.Dal.Model.Company;

    using Property;
    using Property.Activities;
    using User;

    public class Offer : BaseEntity, IAuditableEntity
    {
        public Guid StatusId { get; set; }

        public EnumTypeItem Status { get; set; }

        public Guid? MortgageStatusId { get; set; }

        public EnumTypeItem MortgageStatus { get; set; }

        public Guid? MortgageSurveyStatusId { get; set; }

        public EnumTypeItem MortgageSurveyStatus { get; set; }

        public Guid? SearchStatusId { get; set; }

        public EnumTypeItem SearchStatus { get; set; }

        public Guid? EnquiriesId { get; set; }

        public EnumTypeItem Enquiries { get; set; }

        public Guid? AdditionalSurveyStatusId { get; set; }

        public EnumTypeItem AdditionalSurveyStatus { get; set; }

        public Guid RequirementId { get; set; }

        public virtual Requirement Requirement { get; set; }

        public Guid ActivityId { get; set; }

        public virtual Activity Activity { get; set; }

        public Guid NegotiatorId { get; set; }

        public virtual User Negotiator { get; set; }

        public Guid? BrokerId { get; set; }

        public virtual Contact Broker { get; set; }

        public Guid? BrokerCompanyId { get; set; }

        public virtual Company BrokerCompany { get; set; }

        public Guid? LenderId { get; set; }

        public virtual Contact Lender { get; set; }

        public Guid? LenderCompanyId { get; set; }

        public virtual Company LenderCompany { get; set; }

        public Guid? SurveyorId { get; set; }

        public virtual Contact Surveyor { get; set; }

        public Guid? SurveyorCompanyId { get; set; }

        public virtual Contact SurveyorCompany { get; set; }

        public Guid? AdditionalSurveyorId { get; set; }

        public virtual Contact AdditionalSurveyor { get; set; }

        public Guid? AdditionalSurveyorCompanyId { get; set; }

        public virtual Contact AdditionalSurveyorCompany { get; set; }

        public bool ContractApproved { get; set; }

        public int? MortgageLoanToValue { get; set; }

        public decimal Price { get; set; }

        public DateTime OfferDate { get; set; }

        public DateTime? ExchangeDate { get; set; }

        public DateTime? CompletionDate { get; set; }

        public DateTime? MortgageSurveyDate { get; set; }

        public DateTime? AdditionalSurveyDate { get; set; }

        public string SpecialConditions { get; set; }

        public string ProgressComment { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastModifiedDate { get; set; }
    }
}
