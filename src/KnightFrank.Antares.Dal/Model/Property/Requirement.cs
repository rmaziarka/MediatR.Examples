namespace KnightFrank.Antares.Dal.Model.Property
{
    using System;
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.Dal.Model.Attachment;
    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Offer;

    public class Requirement : BaseEntity
    {
        public DateTime CreateDate { get; set; }

        public List<Contact> Contacts { get; set; } = new List<Contact>();

        public virtual ICollection<RequirementNote> RequirementNotes { get; set; } = new List<RequirementNote>();

        public virtual ICollection<Viewing> Viewings { get; set; } = new List<Viewing>();

        public virtual ICollection<Offer> Offers { get; set; } = new List<Offer>();
       
        public Guid RequirementTypeId { get; set; }

        public virtual RequirementType RequirementType { get; set; }

        public Guid AddressId { get; set; }

        public virtual Address Address { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();

        public decimal? RentMin  { get; set; }

        public decimal? RentMax  { get; set; }
    }
}
