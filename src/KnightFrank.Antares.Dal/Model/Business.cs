﻿namespace KnightFrank.Antares.Dal.Model
{
	using System;

	public class Business : BaseEntity
	{
		public string Name { get; set; }

		public Guid CountryId { get; set; }

		public virtual Country Country { get; set; }
	}
}
