namespace KnightFrank.Antares.Dal.Model.Attributes.Attribute
{
    using System;
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Enum;

    public class FormAttribute : BaseEntity
    {
        public Guid AttributeTypeId { get; set; }

        public virtual EnumTypeItem AttributeType { get; set; }

        public virtual IEnumerable<AttributeLocalized> LabelLocalizeds { get; set; }
    }
}
