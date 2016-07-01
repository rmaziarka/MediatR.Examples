namespace KnightFrank.Antares.Dal.Model.Property.Activities
{
    using System;
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Attachment;
    using KnightFrank.Antares.Dal.Model.Configuration.Property.Activities;
    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Offer;
    using KnightFrank.Antares.Dal.Model.Enum;

    public class Activity : BaseAuditableEntity
    {
        public Activity()
        {
            this.AccessDetails = new ActivityAccessDetails();
            this.AppraisalMeeting = new ActivityAppraisalMeeting();
        }

        public Guid PropertyId { get; set; }

        public virtual Property Property { get; set; }

        public Guid ActivityStatusId { get; set; }

        public EnumTypeItem ActivityStatus { get; set; }

        public Guid ActivityTypeId { get; set; }

        public ActivityType ActivityType { get; set; }

        public virtual ICollection<Contact> Contacts { get; set; }

        public decimal? AgreedInitialMarketingPrice { get; set; }

        // ReSharper disable once InconsistentNaming
        public decimal? KFValuationPrice { get; set; }

        public decimal? VendorValuationPrice { get; set; }

        public decimal? ShortLetPricePerWeek { get; set; }

        public decimal? AskingPrice { get; set; }

        public virtual ICollection<Attachment> Attachments { get; set; }

        public virtual ICollection<Viewing> Viewings { get; set; } = new List<Viewing>();

        public virtual ICollection<ActivityUser> ActivityUsers { get; set; } = new List<ActivityUser>();

        public virtual ICollection<ActivityDepartment> ActivityDepartments { get; set; } = new List<ActivityDepartment>();

        public virtual ICollection<Offer> Offers { get; set; } = new List<Offer>();

        public Guid? SourceId { get; set; }

        public virtual EnumTypeItem Source { get; set; }

        public string SourceDescription { get; set; }

        public Guid? SellingReasonId { get; set; }

        public virtual EnumTypeItem SellingReason { get; set; }

        public string PitchingThreats { get; set; }

        public ActivityAccessDetails AccessDetails { get; set; }

        public ActivityAppraisalMeeting AppraisalMeeting { get; set; }

        public virtual ICollection<ActivityAttendee> ActivityAttendees { get; set; } = new List<ActivityAttendee>();

        public decimal? ServiceChargeAmount { get; set; }
        public string ServiceChargeNote { get; set; }
        public decimal? GroundRentAmount { get; set; }
        public string GroundRentNote { get; set; }

        public string OtherCondition { get; set; }

        public Guid? DisposalTypeId { get; set; }

        public virtual EnumTypeItem DisposalType { get; set; }

        public Guid? DecorationId { get; set; }

        public virtual EnumTypeItem Decoration { get; set; }

        // ReSharper disable once InconsistentNaming
        public decimal? ShortKFValuationPrice { get; set; }

        public decimal? ShortVendorValuationPrice { get; set; }

        public decimal? ShortAgreedInitialMarketingPrice { get; set; }

        // ReSharper disable once InconsistentNaming
        public decimal? LongKFValuationPrice { get; set; }

        public decimal? LongVendorValuationPrice { get; set; }

        public decimal? LongAgreedInitialMarketingPrice { get; set; }
    }
}
