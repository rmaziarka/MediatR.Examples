namespace KnightFrank.Antares.Dal.Model.Resource
{
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Enum;

    public class Locale : BaseEntity
    {
        public string IsoCode { get; set; }

		public virtual ICollection<CountryLocalised> CountryLocaliseds { get; set; }

		public virtual ICollection<EnumLocalised> EnumLocaliseds { get; set; } 

		public virtual ICollection<User.User> Users { get; set; } 
	}
}
