﻿namespace KnightFrank.Antares.Dal.Model.Configuration
{
	using KnightFrank.Antares.Dal.Migrations;

	internal sealed class BusinessConfiguration : BaseEntityConfiguration<Business>
	{
		public BusinessConfiguration()
		{
			this.Property(p => p.Name).HasMaxLength(100);
		}
	}
}
