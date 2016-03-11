namespace KnightFrank.Antares.Dal.Model
{
    using System.Collections.Generic;

    public class Country : BaseEntity
    {
        public string Code { get; set; }
        public virtual ICollection<AddressForm> AddressForms { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
    }
}