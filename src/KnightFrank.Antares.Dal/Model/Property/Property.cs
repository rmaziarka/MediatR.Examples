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
    }
}
