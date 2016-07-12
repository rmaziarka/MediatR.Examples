namespace KnightFrank.Antares.Dal.Model.Configuration.Company
{
    using Model.Company;

    internal sealed class CompanyCategoryLocalisedConfiguration : BaseEntityConfiguration<CompanyCategoryLocalised>
    {
        public CompanyCategoryLocalisedConfiguration()
        {
            this.HasRequired(x => x.Locale).WithMany().HasForeignKey(x => x.LocaleId).WillCascadeOnDelete(false);

            this.Property(c => c.ResourceId).HasColumnName("CompanyCategoryId");

            this.HasRequired(x => x.CompanyCategory).WithMany().HasForeignKey(x => x.ResourceId).WillCascadeOnDelete(false);

            this.Property(x => x.Value).HasMaxLength(100).IsRequired();
        }
    }
}
