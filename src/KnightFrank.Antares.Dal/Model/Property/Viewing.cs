namespace KnightFrank.Antares.Dal.Model.Property
{
    using System;
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using Activities;
    using KnightFrank.Antares.Dal.Model.User;

    public class Viewing : BaseEntity
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string InvitationText { get; set; }

        public string PostViewingComment { get; set; }

        public Guid ActivityId { get; set; }

        public virtual Activity Activity { get; set; }

        public Guid RequirementId { get; set; }

        public virtual Requirement Requirement { get; set; }

        public Guid NegotiatorId { get; set; }

        public virtual User Negotiator { get; set; }

        public List<Contact> Attendees { get; set; } = new List<Contact>();
    }
}
