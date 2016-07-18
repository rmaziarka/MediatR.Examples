namespace KnightFrank.Antares.Dal.Model.Configuration.Company
{
    using KnightFrank.Antares.Dal.Model.Company;

    internal sealed class CompanyConfiguration : BaseEntityConfiguration<Company>
    {
        public CompanyConfiguration()
        {
            this.Property(p => p.Name).HasMaxLength(128).IsRequired();

            this.Property(p => p.WebsiteUrl)
                .HasMaxLength(2500)
                .IsOptional();

            this.Property(p => p.ClientCarePageUrl)
                .HasMaxLength(2500)
                .IsOptional();

            this.Property(p => p.Description)
                .HasMaxLength(4000)
                .IsOptional();

            this.HasOptional(x => x.ClientCareStatus)
                .WithMany()
                .HasForeignKey(x => x.ClientCareStatusId)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.CompanyCategory)
               .WithMany()
               .HasForeignKey(x => x.CompanyCategoryId)
               .WillCascadeOnDelete(false);

            this.HasOptional(x => x.CompanyType)
              .WithMany()
              .HasForeignKey(x => x.CompanyTypeId)
              .WillCascadeOnDelete(false);
        }
    }
}