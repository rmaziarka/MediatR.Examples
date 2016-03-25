namespace KnightFrank.Antares.Dal.Model.Property
{
    using System;
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Contact;
    using KnightFrank.Antares.Dal.Model.Enum;

    public class Activity : BaseAuditableEntity
    {
        public Guid PropertyId { get; set; }

        public Property Property { get; set; }

        public Guid ActivityTypeId { get; set; }

        public virtual EnumTypeItem ActivityType { get; set; }

        public Guid ActivityStatusId { get; set; }

        public EnumTypeItem ActivityStatus { get; set; }

        public virtual ICollection<Contact> Contacts { get; set; }
    }
}
