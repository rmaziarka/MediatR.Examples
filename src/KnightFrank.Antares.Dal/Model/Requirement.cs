namespace KnightFrank.Antares.Dal.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Requirement : BaseEntity
    {
        public DateTime CreateDate { get; set; }

        public virtual ICollection<Contact> Contacts { get; set; }
    }
}