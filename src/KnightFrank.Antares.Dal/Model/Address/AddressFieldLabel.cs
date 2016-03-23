namespace KnightFrank.Antares.Dal.Model.Address
{
    using System;

    public class AddressFieldLabel : BaseEntity
    {
        public virtual AddressField AddressField { get; set; }

        public Guid AddressFieldId { get; set; }

        public string LabelKey { get; set; }

    }
}
