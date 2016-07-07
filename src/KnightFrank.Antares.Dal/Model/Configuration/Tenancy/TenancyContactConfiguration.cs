namespace KnightFrank.Antares.Dal.Model.Configuration.Tenancy
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Infrastructure.Annotations;

    using KnightFrank.Antares.Dal.Model.Tenancy;

    internal sealed class TenancyContactConfiguration : BaseEntityConfiguration<TenancyContact>
    {
        public TenancyContactConfiguration()
        {
            this.HasRequired(x => x.ContactType).WithMany().WillCascadeOnDelete(false);
            this.HasRequired(x => x.Contact).WithMany().HasForeignKey(x => x.ContactId).WillCascadeOnDelete(false);

            var uniqueIndexName = "IX_TenancyId_ContactId";
            this.Property(an => an.TenancyId)
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(new IndexAttribute(uniqueIndexName, 1)
                    {
                        IsUnique = true
                    }));

            this.Property(an => an.ContactId)
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(new IndexAttribute(uniqueIndexName, 2)
                    {
                        IsUnique = true
                    }));
        }
    }
}
