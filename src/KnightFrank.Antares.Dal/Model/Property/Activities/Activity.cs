namespace KnightFrank.Antares.Dal.Model.Property.Activities
{
    using System;
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Attachment;
    using KnightFrank.Antares.Dal.Model.Company;
    using KnightFrank.Antares.Dal.Model.Configuration.Property.Activities;
    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Offer;
    using KnightFrank.Antares.Dal.Model.Enum;

    public class Activity : BaseAuditableEntity
    {
        public Guid PropertyId { get; set; }

        public virtual Property Property { get; set; }

        public Guid ActivityStatusId { get; set; }

        public EnumTypeItem ActivityStatus { get; set; }

        public Guid ActivityTypeId { get; set; }

        public ActivityType ActivityType { get; set; }

        public virtual ICollection<Contact> Contacts { get; set; } = new List<Contact>();

        public decimal? AgreedInitialMarketingPrice { get; set; }

        public decimal? KfValuationPrice { get; set; }

        public decimal? VendorValuationPrice { get; set; }

        public decimal? ShortLetPricePerWeek { get; set; }

        public decimal? AskingPrice { get; set; }

        public virtual ICollection<Attachment> Attachments { get; set; }

        public virtual ICollection<Viewing> Viewings { get; set; } = new List<Viewing>();

        public virtual ICollection<ActivityUser> ActivityUsers { get; set; } = new List<ActivityUser>();

        public virtual ICollection<ActivityDepartment> ActivityDepartments { get; set; } = new List<ActivityDepartment>();

        public virtual ICollection<Offer> Offers { get; set; } = new List<Offer>();

        public Guid? SolicitorId { get; set; }

        public virtual Contact Solicitor { get; set; }

        public Guid? SolicitorCompanyId { get; set; }

        public virtual Company SolicitorCompany { get; set; }

        public Guid? SourceId { get; set; }

        public virtual EnumTypeItem Source { get; set; }

        public string SourceDescription { get; set; }

        public Guid? SellingReasonId { get; set; }

        public virtual EnumTypeItem SellingReason { get; set; }

        public string PitchingThreats { get; set; }

        public string KeyNumber { get; set; }

        public string AccessArrangements { get; set; }

        public DateTime? AppraisalMeetingStart { get; set; }

        public DateTime? AppraisalMeetingEnd { get; set; }

        public string AppraisalMeetingInvitationText { get; set; }

        public virtual ICollection<ActivityAttendee> AppraisalMeetingAttendees { get; set; } = new List<ActivityAttendee>();

        public decimal? ServiceChargeAmount { get; set; }
        public string ServiceChargeNote { get; set; }
        public decimal? GroundRentAmount { get; set; }
        public string GroundRentNote { get; set; }

        public string OtherCondition { get; set; }

        public Guid? DisposalTypeId { get; set; }

        public virtual EnumTypeItem DisposalType { get; set; }

        public Guid? DecorationId { get; set; }

        public virtual EnumTypeItem Decoration { get; set; }

        public decimal? ShortKfValuationPrice { get; set; }

        public decimal? ShortVendorValuationPrice { get; set; }

        public decimal? ShortAgreedInitialMarketingPrice { get; set; }

        public decimal? LongKfValuationPrice { get; set; }

        public decimal? LongVendorValuationPrice { get; set; }

        public decimal? LongAgreedInitialMarketingPrice { get; set; }

        public Guid? PriceTypeId { get; set; }

        public virtual EnumTypeItem PriceType { get; set; }

        public decimal? Price { get; set; }

        public Guid? MatchFlexibilityId { get; set; }

        public virtual EnumTypeItem MatchFlexibility { get; set; }

        public decimal? MatchFlexValue { get; set; }

        public decimal? MatchFlexPercentage { get; set; }

        public Guid? RentPaymentPeriodId { get; set; }

        public virtual EnumTypeItem RentPaymentPeriod { get; set; }

        public decimal? ShortAskingWeekRent { get; set; }

        public decimal? ShortAskingMonthRent { get; set; }

        public decimal? LongAskingWeekRent { get; set; }

        public decimal? LongAskingMonthRent { get; set; }

        public Guid? ShortMatchFlexibilityId { get; set; }

        public virtual EnumTypeItem ShortMatchFlexibility { get; set; }

        public decimal? ShortMatchFlexWeekValue { get; set; }

        public decimal? ShortMatchFlexMonthValue { get; set; }

        public decimal? ShortMatchFlexPercentage { get; set; }

        public Guid? LongMatchFlexibilityId { get; set; }

        public virtual EnumTypeItem LongMatchFlexibility { get; set; }

        public decimal? LongMatchFlexWeekValue { get; set; }

        public decimal? LongMatchFlexMonthValue { get; set; }

        public decimal? LongMatchFlexPercentage { get; set; }
    }
}
