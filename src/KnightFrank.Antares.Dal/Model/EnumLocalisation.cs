namespace KnightFrank.Antares.Dal.Model
{
    using System;

    public class EnumLocalisation : BaseEntity
    {
        public Guid EnumTypeItemId { get; set; }

        public virtual EnumTypeItem EnumTypeItem { get; set; }

        public Guid LocalId { get; set; }

        public virtual Local Local { get; set; }

        public string Value { get; set; }
    }
}
