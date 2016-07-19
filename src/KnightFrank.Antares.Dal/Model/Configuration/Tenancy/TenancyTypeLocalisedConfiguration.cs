namespace KnightFrank.Antares.Dal.Model.Configuration.Tenancy
{
    using KnightFrank.Antares.Dal.Model.Tenancy;
    internal sealed class TenancyTypeLocalisedConfiguration : BaseEntityConfiguration<TenancyTypeLocalised>
    {
        public TenancyTypeLocalisedConfiguration()
        {
            this.HasRequired(x => x.Locale).WithMany().HasForeignKey(x => x.LocaleId).WillCascadeOnDelete(false);

            this.Property(c => c.ResourceId).HasColumnName("TenancyTypeId");

            this.HasRequired(x => x.TenancyType).WithMany().HasForeignKey(x => x.ResourceId).WillCascadeOnDelete(false);

            this.Property(x => x.Value).HasMaxLength(100).IsRequired();
        }
    }
}
