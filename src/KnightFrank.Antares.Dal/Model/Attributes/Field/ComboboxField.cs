namespace KnightFrank.Antares.Dal.Model.Attributes.Field
{
    using System;

    using KnightFrank.Antares.Dal.Model.Enum;

    public class ComboboxField : Field
    {
        public Guid EnumTypeId { get; set; }
        public virtual EnumType EnumType { get; set; }
    }
}
