namespace KnightFrank.Antares.Dal.Migrations
{
	using KnightFrank.Antares.Dal.Model;

    internal sealed class CountryLocalisedConfiguration : BaseEntityConfiguration<CountryLocalised>
    {
        public CountryLocalisedConfiguration()
        {
            this.Property(r => r.Value)
                .HasMaxLength(100);
        }
    }
}