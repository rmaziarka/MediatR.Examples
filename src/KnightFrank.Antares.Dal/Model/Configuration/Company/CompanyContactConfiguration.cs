namespace KnightFrank.Antares.Dal.Model.Configuration.Company
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Infrastructure.Annotations;

    using KnightFrank.Antares.Dal.Model.Company;

    internal sealed class CompanyContactConfiguration : BaseEntityConfiguration<CompanyContact>
    {
        public CompanyContactConfiguration()
        {
            this.HasRequired(t => t.Contact)
                .WithMany(t => t.CompaniesContacts)
                .HasForeignKey(t => t.ContactId);

            this.HasRequired(t => t.Company)
                .WithMany(t => t.CompaniesContacts)
                .HasForeignKey(t => t.CompanyId);

            var uniqueIndexName = "IX_CompanyId_ContactId";
            this.Property(an => an.CompanyId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute(uniqueIndexName, 1) { IsUnique = true }));

            this.Property(an => an.ContactId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute(uniqueIndexName, 2) { IsUnique = true }));
        }
    }
}