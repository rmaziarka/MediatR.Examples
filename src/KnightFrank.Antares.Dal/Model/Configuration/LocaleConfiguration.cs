namespace KnightFrank.Antares.Dal.Migrations
{
	using KnightFrank.Antares.Dal.Model;

    internal sealed class LocaleConfiguration : BaseEntityConfiguration<Locale>
    {
        public LocaleConfiguration()
        {
            this.Property(r => r.IsoCode)
                .HasMaxLength(2);

			this.HasMany(p => p.CountryLocaliseds)
				.WithRequired(p => p.Locale)
				.WillCascadeOnDelete(false);

			this.HasMany(p => p.EnumLocaliseds)
				.WithRequired(p => p.Locale)
				.WillCascadeOnDelete(false);
		}
	}
}