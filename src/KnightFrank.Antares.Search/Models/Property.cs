namespace KnightFrank.Antares.Search.Models
{
    using System.Collections.Generic;

    public class Property
    {
        public string Id { get; set; }

        public string AddressId { get; set; }

        public Address Address { get; set; }

        public string PropertyTypeId { get; set; }

        public PropertyType PropertyType { get; set; }

        public string DivisionId { get; set; }

        public EnumTypeItem Division { get; set; }

        public string AttributeValuesId { get; set; }

        public AttributeValues AttributeValues { get; set; }

        public double TotalAreaBreakdown { get; set; }

        public IList<Ownership> Ownerships { get; set; }
    }
}
