namespace KnightFrank.Antares.Dal.Model.Configuration.Resource
{
    using KnightFrank.Antares.Dal.Model.Resource;

    internal sealed class CountryLocalisedConfiguration : BaseEntityConfiguration<CountryLocalised>
    {
        public CountryLocalisedConfiguration()
        {
            this.Property(r => r.Value)
                .HasMaxLength(100);
        }
    }
}