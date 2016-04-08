namespace KnightFrank.Antares.Dal.Model.Property
{
    using System;

    using Attribute = KnightFrank.Antares.Dal.Model.Attribute.Attribute;

    public class PropertyAttributeFormDefinition:BaseEntity
    {
        public Guid AttributeId { get; set; }

        public Attribute Attribute { get; set; }

        public Guid PropertyAttributeFormId { get; set; }

        public PropertyAttributeForm PropertyAttributeForm { get; set; }

        public int Order { get; set; }
    }
}
