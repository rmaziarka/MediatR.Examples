namespace KnightFrank.Antares.Dal.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Requirement : BaseEntity
    {
        [Required]
        public DateTime CreateDate { get; set; }

        public ICollection<Contact> Contacts { get; set; }
    }
}