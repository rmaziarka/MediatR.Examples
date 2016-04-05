namespace KnightFrank.Antares.Dal.Model.Attributes.FormDefinition
{
    using System;
    using KnightFrank.Antares.Dal.Model.Attributes.Attribute;

    public abstract class FormDefinition : BaseEntity
    {
        public int ColumnOrder { get; set; }

        public int RowOrder { get; set; }

        public bool Required { get; set; }

        public Guid AttributeId { get; set; }

        public virtual FormAttribute Attribute { get; set; }
    }
}
