namespace KnightFrank.Antares.Dal.Model
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Property : BaseEntity
    {
        public Guid AddressId { get; set; }

        public virtual Address Address { get; set; }
    }
}
