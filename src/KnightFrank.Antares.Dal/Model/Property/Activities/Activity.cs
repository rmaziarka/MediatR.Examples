namespace KnightFrank.Antares.Dal.Model.Property.Activities
{
    using System;
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Attachment;
    using KnightFrank.Antares.Dal.Model.Company;
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

        public virtual ICollection<Contact> Contacts { get; set; }

        public decimal? MarketAppraisalPrice { get; set; }

        public decimal? RecommendedPrice { get; set; }

        public decimal? VendorEstimatedPrice { get; set; }

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
    }
}
