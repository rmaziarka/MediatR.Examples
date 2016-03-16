namespace KnightFrank.Antares.Dal.Model
{
	using System;

	public class CountryLocalised : BaseEntity
	{
		public Guid CountryId { get; set; }

		public virtual Country Country { get; set; }

		public Guid LocaleId { get; set; }

		public virtual Locale Locale { get; set; }

		public string Value { get; set; }
	}
}