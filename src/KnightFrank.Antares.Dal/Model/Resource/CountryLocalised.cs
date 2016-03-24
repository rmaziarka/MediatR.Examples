namespace KnightFrank.Antares.Dal.Model.Resource
{
    using System;

    using KnightFrank.Antares.Dal.Model.Common;

    public class CountryLocalised : BaseEntity, ILocaled
    {
		public Guid CountryId { get; set; }

		public virtual Country Country { get; set; }

		public Guid LocaleId { get; set; }

		public virtual Locale Locale { get; set; }

		public string Value { get; set; }
	}
}