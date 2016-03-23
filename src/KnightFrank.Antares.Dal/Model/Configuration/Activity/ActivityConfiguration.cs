namespace KnightFrank.Antares.Dal.Model.Configuration.Activity
{
    using KnightFrank.Antares.Dal.Model.Activity;

    internal sealed class ActivityConfiguration : BaseEntityConfiguration<Activity>
    {
        public ActivityConfiguration()
        {
            this.HasRequired(a => a.Property)
                .WithMany(p => p.Activities)
                .HasForeignKey(a => a.PropertyId)
                .WillCascadeOnDelete(false);

            this.HasRequired(a => a.ActivityType).WithMany().HasForeignKey(s => s.ActivityTypeId).WillCascadeOnDelete(false);

            this.HasRequired(a => a.ActivityStatus).WithMany().HasForeignKey(s => s.ActivityStatusId).WillCascadeOnDelete(false);
            
            this.HasMany(a => a.Contacts).WithMany().Map(
                cs =>
                    {
                        cs.MapLeftKey("ActivityId");
                        cs.MapRightKey("ContactId");
                    });
        }
    }
}
