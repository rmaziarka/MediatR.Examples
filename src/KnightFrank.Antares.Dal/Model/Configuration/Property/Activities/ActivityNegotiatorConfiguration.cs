namespace KnightFrank.Antares.Dal.Model.Configuration.Property.Activities
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Infrastructure.Annotations;

    using KnightFrank.Antares.Dal.Model.Property.Activities;

    internal sealed class ActivityNegotiatorConfiguration : BaseEntityConfiguration<ActivityNegotiator>
    {
        public ActivityNegotiatorConfiguration()
        {
            this.HasRequired(an => an.Activity).WithMany(a => a.ActivityNegotiators).HasForeignKey(an => an.ActivityId).WillCascadeOnDelete(false);
            this.HasRequired(an => an.Negotiator).WithMany().HasForeignKey(an => an.NegotiatorId).WillCascadeOnDelete(false);

            var uniqueIndexName = "IX_ActivityId_NegotiatorId";
            this.Property(an => an.ActivityId)
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(new IndexAttribute(uniqueIndexName, 1)
                    {
                        IsUnique = true
                    }));

            this.Property(an => an.NegotiatorId)
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(new IndexAttribute(uniqueIndexName, 2)
                    {
                        IsUnique = true
                    }));
        }
    }
}