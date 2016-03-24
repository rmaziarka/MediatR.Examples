namespace KnightFrank.Antares.Dal.Model.Property
{
    using System;

    using KnightFrank.Antares.Dal.Model.Resource;

    public class PropertyTypeLocalised : BaseEntity
    {
        public Guid PropertyTypeId { get; set; }
        public virtual PropertyType PropertyType { get; set; }
        public Guid LocaleId { get; set; }
        public virtual Locale Locale { get; set; }
        public string Value { get; set; }
    }
}
