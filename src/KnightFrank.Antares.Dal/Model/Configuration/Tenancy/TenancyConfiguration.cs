namespace KnightFrank.Antares.Dal.Model.Configuration.Tenancy
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Infrastructure.Annotations;

    using KnightFrank.Antares.Dal.Model.Tenancy;

    internal sealed class TenancyConfiguration : BaseEntityConfiguration<Tenancy>
    {
        public TenancyConfiguration()
        {
            this.HasMany(x => x.Terms)
                .WithRequired()
                .HasForeignKey(x => x.TenancyId)
                .WillCascadeOnDelete(false);

            this.HasRequired(x => x.TenancyType)
                .WithMany()
                .HasForeignKey(x => x.TenancyTypeId)
                .WillCascadeOnDelete(false);

            this.HasRequired(x => x.Requirement).WithMany().HasForeignKey(x => x.RequirementId).WillCascadeOnDelete(false);

            this.HasMany(x=> x.Contacts).WithRequired().HasForeignKey(x=> x.TenancyId).WillCascadeOnDelete(false);

            var uniqueIndexName = "IX_RequirementId_ActivityId";
            this.Property(an => an.RequirementId)
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(new IndexAttribute(uniqueIndexName, 1)
                    {
                        IsUnique = true
                    }));

            this.Property(an => an.ActivityId)
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(new IndexAttribute(uniqueIndexName, 2)
                    {
                        IsUnique = true
                    }));
        }
    }
}
