namespace KnightFrank.Antares.Dal.Model.Contacts
{
    using System;

    using Resource;

    public class ContactTitle : BaseEntity
    {
        public string Title { get; set; }

        public virtual Locale Locale { get; set; }

        public Guid LocaleId { get; set; }

        public int? Priority { get; set; }
    }
}
