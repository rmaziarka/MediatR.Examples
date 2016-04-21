namespace KnightFrank.Antares.Dal.Model.Configuration.Resource
{
    using KnightFrank.Antares.Dal.Model.Resource;

    internal sealed class CountryLocalisedConfiguration : BaseEntityConfiguration<CountryLocalised>
    {
        public CountryLocalisedConfiguration()
        {
            this.HasRequired(x => x.Locale).WithMany().HasForeignKey(x => x.LocaleId).WillCascadeOnDelete(false);

            this.Property(c => c.ResourceId).HasColumnName("CountryId");

            this.HasRequired(c => c.Country)
                .WithMany(t => t.CountryLocaliseds)
                .HasForeignKey(c => c.ResourceId)
                .WillCascadeOnDelete(false);

            this.Property(r => r.Value).HasMaxLength(100).IsRequired();
        }
    }
}
