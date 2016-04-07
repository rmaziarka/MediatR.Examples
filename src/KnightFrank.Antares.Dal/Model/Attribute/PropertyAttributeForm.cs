namespace KnightFrank.Antares.Dal.Model.Attribute
{
    using System;
    using System.Collections.Generic;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Resource;

    public class PropertyAttributeForm: BaseEntity
    {
        public Guid CountryId { get; set; }

        public virtual Country Country { get; set; }

        public Guid PropertyTypeId { get; set; }

        public virtual PropertyType PropertyType { get; set; }
        public virtual ICollection<PropertyAttributeFormDefinition> PropertyAttributeFormDefinitions { get; set; }
    }
}
