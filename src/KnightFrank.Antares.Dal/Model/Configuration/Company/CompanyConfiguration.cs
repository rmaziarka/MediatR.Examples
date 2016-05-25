namespace KnightFrank.Antares.Dal.Model.Configuration.Company
{
    using KnightFrank.Antares.Dal.Model.Company;

    internal sealed class CompanyConfiguration : BaseEntityConfiguration<Company>
    {
        public CompanyConfiguration()
        {
            this.Property(p => p.Name).HasMaxLength(128).IsRequired();

            this.Property(p => p.WebsiteUrl)
                .HasMaxLength(1000)
                .IsOptional();

            this.Property(p => p.ClientCarePageUrl)
                .HasMaxLength(1000)
                .IsOptional();

            this.HasOptional(x => x.ClientCareStatus)
                .WithMany()
                .HasForeignKey(x => x.ClientCareStatusId)
                .WillCascadeOnDelete(false);

            this.HasMany(p => p.Contacts)
                .WithMany()
                .Map(cs =>
                {
                    cs.MapLeftKey("CompanyId");
                    cs.MapRightKey("ContactId");
                });
        }
    }
}