using System;

namespace KnightFrank.Antares.Dal.Model.PropertyType
{
    using KnightFrank.Antares.Dal.Model.Enum;

    public class PropertyTypeDefinition : BaseEntity
    {
        public Guid PropertyTypeId { get; set; }
        public PropertyType PropertyType { get; set; }
        public Guid CountryId { get; set; }
        public Country Country { get; set; }
        public Guid DivisionId { get; set; }
        public EnumTypeItem Division { get; set; }
    }
}
