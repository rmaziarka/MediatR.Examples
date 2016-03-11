namespace KnightFrank.Antares.Dal.Model
{
    using System;

    public class AddressFormEntityType : BaseEntity
    {
        public Guid AddressFormId { get; set; }
        public virtual AddressForm AddressForm { get; set; }
        public Guid EnumTypeItemId { get; set; }
        public virtual EnumTypeItem EnumTypeItem { get; set; }
    }
}