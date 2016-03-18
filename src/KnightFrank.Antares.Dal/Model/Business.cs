namespace KnightFrank.Antares.Dal.Model
{
	using System;
	using System.Collections.Generic;

	public class Business : BaseEntity
	{
		public string Name { get; set; }

		public Guid CountryId { get; set; }

		public virtual Country Country { get; set; }

		public virtual ICollection<User> Users { get; set; }
	}
}
