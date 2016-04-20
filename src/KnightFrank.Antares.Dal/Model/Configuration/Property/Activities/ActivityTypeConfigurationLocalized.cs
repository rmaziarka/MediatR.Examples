namespace KnightFrank.Antares.Dal.Model.Configuration.Property.Activities
{
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;

    internal sealed class ActivityTypeConfigurationLocalized : BaseEntityConfiguration<ActivityTypeLocalized>
    {
        public ActivityTypeConfigurationLocalized()
        {
            this.HasRequired(x => x.Locale)
                .WithMany()
                .HasForeignKey(x => x.LocaleId)
                .WillCascadeOnDelete(false);

            this.Property(c => c.ResourceId).HasColumnName("ActivityTypeId");

            this.HasRequired(x => x.ActivityType)
                .WithMany()
                .HasForeignKey(x => x.ResourceId)
                .WillCascadeOnDelete(false);

            this.Property(x => x.Value)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
