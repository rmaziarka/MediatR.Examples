namespace KnightFrank.Antares.Dal.Model
{
    using System;

    public class EnumLocalised : BaseEntity
    {
        public Guid EnumTypeItemId { get; set; }

        public virtual EnumTypeItem EnumTypeItem { get; set; }

        public Guid LocaleId { get; set; }

        public virtual Locale Locale { get; set; }

        public string Value { get; set; }
    }
}
