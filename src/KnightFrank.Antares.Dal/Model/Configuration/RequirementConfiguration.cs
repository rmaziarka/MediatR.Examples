namespace KnightFrank.Antares.Dal.Migrations
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using KnightFrank.Antares.Dal.Model;

    internal sealed class RequirementConfiguration : BaseEntityConfiguration<Requirement>
    {
        public RequirementConfiguration()
        {
            this.HasMany(r => r.Contacts)
                .WithMany(c => c.Requirements)
                .Map(cs =>
                {
                    cs.MapLeftKey("RequirementId");
                    cs.MapRightKey("ContactId");
                });

            this.Property(r => r.CreateDate)
                .IsRequired();

            this.Property(r => r.Description)
                .HasMaxLength(4000);
        }
    }
}