namespace KnightFrank.Antares.Dal.Model
{
    using System.Collections.Generic;

    public class Country : BaseEntity
    {
        public string IsoCode { get; set; }

		public virtual ICollection<AddressForm> AddressForms { get; set; }

		public virtual ICollection<Address> Addresses { get; set; }

		public virtual ICollection<CountryLocalised> CountryLocaliseds { get; set; }

		public virtual ICollection<Business> Businesses { get; set; }

		public virtual ICollection<Department> Departments { get; set; }

		public virtual ICollection<User> Users { get; set; }
	}
}