namespace KnightFrank.Antares.Dal.Model.Attributes.Field
{
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Enum;

    public class Field : BaseEntity
    {
        public string FieldName { get; set; }

        public virtual IEnumerable<FieldLocalized> PlaceholderLocalizeds { get; set; }

        public int EntityTypeId { get; set; }
        public virtual EnumTypeItem EntityType { get; set; }

        public int StorageTypeId { get; set; }
        public virtual EnumTypeItem StorageType { get; set; }

        public int UiFieldTypeId { get; set; }
        public virtual EnumTypeItem UiFieldType { get; set; }

        public int DataTypeId { get; set; }
        public virtual EnumTypeItem DataType { get; set; }
    }
}
