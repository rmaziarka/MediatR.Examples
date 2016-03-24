namespace KnightFrank.Antares.Dal.Model.Address
{
    using System;

    using KnightFrank.Antares.Dal.Model.Resource;

    public class Address : BaseEntity
    {
        public Guid CountryId { get; set; }

        public virtual Country Country { get; set; }

        public Guid AddressFormId { get; set; }

        public virtual AddressForm AddressForm { get; set; }

        public string PropertyName { get; set; }

        public string PropertyNumber { get; set; }

        public string Line1 { get; set; }

        public string Line2 { get; set; }

        public string Line3 { get; set; }

        public string Postcode { get; set; }

        public string City { get; set; }

        public string County { get; set; }
    }
}
