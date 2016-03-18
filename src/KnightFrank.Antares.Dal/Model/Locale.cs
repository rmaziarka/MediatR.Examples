namespace KnightFrank.Antares.Dal.Model
{
	using System.Collections.Generic;

	public class Locale : BaseEntity
    {
        public string IsoCode { get; set; }

		public virtual ICollection<CountryLocalised> CountryLocaliseds { get; set; }

		public virtual ICollection<EnumLocalised> EnumLocaliseds { get; set; } 
	}
}
