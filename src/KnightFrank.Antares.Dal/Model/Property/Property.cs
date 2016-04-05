namespace KnightFrank.Antares.Dal.Model.Property
{
    using System;
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Address;

    public class Property : BaseEntity
    {
        public Guid AddressId { get; set; }

        public virtual Address Address { get; set; }

        public virtual ICollection<Ownership> Ownerships { get; set; } = new List<Ownership>();

        public virtual ICollection<Activity> Activities { get; set; } = new List<Activity>();

        public Guid PropertyTypeId { get; set; }
        
        public virtual PropertyType PropertyType { get; set; }

        public int? MinBedrooms { get; set; }
        public int? MaxBedrooms { get; set; }
        public int? MinReceptions { get; set; }
        public int? MaxReceptions { get; set; }
        public int? MinBathrooms { get; set; }
        public int? MaxBathrooms { get; set; }
        public double? MinArea { get; set; }
        public double? MaxArea { get; set; }
        public double? MinLandArea { get; set; }
        public double? MaxLandArea { get; set; }
        public int? MinGuestRooms { get; set; }
        public int? MaxGuestRooms { get; set; }
        public int? MinFunctionRooms { get; set; }
        public int? MaxFunctionRooms { get; set; }
        public int? MinCarParkingSpaces { get; set; }
        public int? MaxCarParkingSpaces { get; set; }
    }
}
