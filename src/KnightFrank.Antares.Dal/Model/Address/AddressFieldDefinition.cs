namespace KnightFrank.Antares.Dal.Model.Address
{
    using System;

    public class AddressFieldDefinition : BaseEntity
    {
        public Guid AddressFieldId { get; set; }

        public virtual AddressField AddressField { get; set; }

        public Guid AddressFieldLabelId { get; set; }

        public virtual AddressFieldLabel AddressFieldLabel { get; set; }

        public Guid AddressFormId { get; set; }

        public virtual AddressForm AddressForm { get; set; }

        public bool Required { get; set; }

        public string RegEx { get; set; }

        public short RowOrder { get; set; }

        public short ColumnOrder { get; set; }

        public short ColumnSize { get; set; }
    }
}
