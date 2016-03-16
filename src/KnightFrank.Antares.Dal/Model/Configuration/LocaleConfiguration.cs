namespace KnightFrank.Antares.Dal.Migrations
{
	using KnightFrank.Antares.Dal.Model;

    internal sealed class LocaleConfiguration : BaseEntityConfiguration<Locale>
    {
        public LocaleConfiguration()
        {
            this.Property(r => r.IsoCode)
                .HasMaxLength(40);
        }
    }
}