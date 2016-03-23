namespace KnightFrank.Antares.Domain.Requirement.Commands
{
    using System;

    public class CreateOrUpdateRequirementAddress
    {
        public Guid CountryId { get; set; }

        public Guid AddressFormId { get; set; }

        public string Postcode { get; set; }

        public string City { get; set; }

        public string Line2 { get; set; }
    }
}
