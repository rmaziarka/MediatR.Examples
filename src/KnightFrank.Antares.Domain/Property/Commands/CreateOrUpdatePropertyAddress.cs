namespace KnightFrank.Antares.Domain.Property.Commands
{
    using System;

    public class CreateOrUpdatePropertyAddress
    {
        public Guid CountryId { get; set; }

        public Guid AddressFormId { get; set; }

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