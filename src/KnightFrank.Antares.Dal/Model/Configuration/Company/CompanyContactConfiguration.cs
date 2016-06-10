namespace KnightFrank.Antares.Dal.Model.Configuration.Company
{
    using KnightFrank.Antares.Dal.Model.Company;

    internal sealed class CompanyContactConfiguration : BaseEntityConfiguration<CompanyContact>
    {
        public CompanyContactConfiguration()
        {
            this.HasKey(q =>
                new
                {
                    q.CompanyId,
                    q.ContactId
                });

            this.HasRequired(t => t.Contact)
                .WithMany(t => t.CompaniesContacts)
                .HasForeignKey(t => t.ContactId);

            this.HasRequired(t => t.Company)
                .WithMany(t => t.CompaniesContacts)
                .HasForeignKey(t => t.CompanyId);
        }
    }
}