namespace KnightFrank.Antares.Dal.Model.Property.Activities
{
    using System;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.User;

    public class ActivityAttendee : BaseEntity
    {
        public Guid ActivityId { get; set; }

        public virtual Activity Activity { get; set; }

        public Guid? UserId { get; set; }

        public virtual User User { get; set; }

        public Guid? ContactId { get; set; }

        public virtual Contact Contact { get; set; }
    }
}
