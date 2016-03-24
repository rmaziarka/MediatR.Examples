using System;

namespace KnightFrank.Antares.Dal.Model.PropertyType
{
    using KnightFrank.Antares.Dal.Model.Resource;

    public class PropertyTypeLocalised : BaseEntity
    {
        public Guid PropertyTypeId { get; set; }
        public PropertyType PropertyType { get; set; }
        public Guid LocaleId { get; set; }
        public Locale Locale { get; set; }
        public string Value { get; set; }
    }
}
