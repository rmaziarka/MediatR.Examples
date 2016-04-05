namespace KnightFrank.Antares.Dal.Model.Attributes.Field
{
    using System;

    using KnightFrank.Antares.Dal.Model.Resource;

    public class FieldLocalized : BaseEntity
    {
        public Guid FieldId { get; set; }

        public virtual Field Field { get; set; }

        public Guid LocaleId { get; set; }

        public virtual Locale Locale { get; set; }

        public string Value { get; set; }
    }
}
