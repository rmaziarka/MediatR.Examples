namespace KnightFrank.Antares.Dal.Model.Configuration.Property.Activities
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Infrastructure.Annotations;

    using KnightFrank.Antares.Dal.Model.Property.Activities;

    internal sealed class ActivityUserConfiguration : BaseEntityConfiguration<ActivityUser>
    {
        public ActivityUserConfiguration()
        {
            this.HasRequired(an => an.Activity).WithMany(a => a.ActivityUsers).HasForeignKey(an => an.ActivityId).WillCascadeOnDelete(false);
            this.HasRequired(an => an.User).WithMany().HasForeignKey(an => an.UserId).WillCascadeOnDelete(false);

            var uniqueIndexName = "IX_ActivityId_UserId";
            this.Property(an => an.ActivityId)
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
        }
    }
}