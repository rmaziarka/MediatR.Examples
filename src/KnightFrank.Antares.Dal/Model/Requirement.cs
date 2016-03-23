namespace KnightFrank.Antares.Dal.Model
{
    using System;
    using System.Collections.Generic;

    public class Requirement : BaseEntity
    {
        public Requirement()
        {
            this.Contacts = new List<Contact>();
        }

        public DateTime CreateDate { get; set; }

        public List<Contact> Contacts { get; set; }

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
    }
}