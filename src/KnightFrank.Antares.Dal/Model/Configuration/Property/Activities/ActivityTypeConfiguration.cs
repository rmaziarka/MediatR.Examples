namespace KnightFrank.Antares.Dal.Model.Configuration.Property.Activities
{
    using KnightFrank.Antares.Dal.Model.Property.Activities;

    internal sealed class ActivityTypeConfiguration : BaseEntityConfiguration<ActivityType>
    {
        public ActivityTypeConfiguration()
        {
            this.Property(x => x.Code)
                .HasMaxLength(50)
                .IsRequired();

            this.Property(x => x.EnumCode)
                .HasMaxLength(250)
                .IsRequired()
                .IsUnique();
        }
    }
}
