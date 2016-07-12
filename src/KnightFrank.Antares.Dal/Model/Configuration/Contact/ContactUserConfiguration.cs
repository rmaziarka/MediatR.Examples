namespace KnightFrank.Antares.Dal.Model.Configuration.Contact
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Infrastructure.Annotations;

    using KnightFrank.Antares.Dal.Model.Contacts;

    internal sealed class ContactUserConfiguration : BaseEntityConfiguration<ContactUser>
    {
        public ContactUserConfiguration()
        {
            this.HasRequired(an => an.Contact).WithMany(a => a.ContactUsers).HasForeignKey(an => an.ContactId).WillCascadeOnDelete(false);
            this.HasRequired(an => an.User).WithMany().HasForeignKey(an => an.UserId).WillCascadeOnDelete(false);

            var uniqueIndexName = "IX_ContactId_UserId";
            this.Property(an => an.ContactId)
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(new IndexAttribute(uniqueIndexName, 1)
                    {
                        IsUnique = true
                    }));

            this.Property(an => an.UserId)
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(new IndexAttribute(uniqueIndexName, 2)
                    {
                        IsUnique = true
                    }));

            this.HasRequired(a => a.UserType).WithMany().WillCascadeOnDelete(false);
        }
    }
}