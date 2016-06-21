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

        public Guid AddressId { get; set; }

        public virtual Address Address { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public int? MinBedrooms { get; set; }

        public int? MaxBedrooms { get; set; }

        public int? MinReceptionRooms { get; set; }

        public int? MaxReceptionRooms { get; set; }

        public int? MinBathrooms { get; set; }

        public int? MaxBathrooms { get; set; }

        public int? MinParkingSpaces { get; set; }

        public int? MaxParkingSpaces { get; set; }

        public double? MinArea { get; set; }

        public double? MaxArea { get; set; }

        public double? MinLandArea { get; set; }

        public double? MaxLandArea { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
    }
}
