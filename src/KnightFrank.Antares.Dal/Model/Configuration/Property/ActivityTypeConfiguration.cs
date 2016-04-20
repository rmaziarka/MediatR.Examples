namespace KnightFrank.Antares.Dal.Model.Configuration.Property
{
    using KnightFrank.Antares.Dal.Model.Property;

    internal sealed class ActivityTypeConfiguration : BaseEntityConfiguration<ActivityType>
    {
        public ActivityTypeConfiguration()
        {

            this.Property(x => x.Code)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}
