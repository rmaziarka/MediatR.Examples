namespace KnightFrank.Antares.Dal.Model.Address
{
    using System;

    using KnightFrank.Antares.Dal.Model.Enum;

    public class AddressFormEntityType : BaseEntity
    {
        public Guid AddressFormId { get; set; }

        public virtual AddressForm AddressForm { get; set; }

        public Guid EnumTypeItemId { get; set; }

        public virtual EnumTypeItem EnumTypeItem { get; set; }
    }
}
