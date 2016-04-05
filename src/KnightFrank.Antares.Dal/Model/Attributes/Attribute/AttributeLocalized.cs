namespace KnightFrank.Antares.Dal.Model.Attributes.Attribute
{
    using System;

    using KnightFrank.Antares.Dal.Model.Resource;

    public class AttributeLocalized: BaseEntity
    {
        public Guid AttributeId { get; set; }

        public virtual FormAttribute Attribute { get; set; }

        public Guid LocaleId { get; set; }

        public virtual Locale Locale { get; set; }
        
        public string Value { get; set; }
    }
}
