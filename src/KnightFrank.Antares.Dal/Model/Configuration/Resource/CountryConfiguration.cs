namespace KnightFrank.Antares.Dal.Model.Configuration.Resource
{
    using KnightFrank.Antares.Dal.Model.Resource;

    internal sealed class CountryConfiguration : BaseEntityConfiguration<Country>
    {
        public CountryConfiguration()
        {
            this.Property(p => p.IsoCode).HasMaxLength(2);

            this.HasMany(p => p.AddressForms)
                .WithRequired(p => p.Country)
                .WillCascadeOnDelete(false);

            this.HasMany(p => p.Addresses)
                .WithRequired(p => p.Country)
                .WillCascadeOnDelete(false);

			this.HasMany(p => p.Businesses)
				.WithRequired(p => p.Country)
				.WillCascadeOnDelete(false);

			this.HasMany(p => p.Departments)
				.WithRequired(p => p.Country)
				.WillCascadeOnDelete(false);

			this.HasMany(p => p.Users)
				.WithRequired(p => p.Country)
				.WillCascadeOnDelete(false);
		}
	}
}