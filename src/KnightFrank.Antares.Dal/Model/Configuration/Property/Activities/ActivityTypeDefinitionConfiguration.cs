namespace KnightFrank.Antares.Dal.Model.Configuration.Property.Activities
{
    using KnightFrank.Antares.Dal.Model.Property.Activities;

    internal sealed class ActivityTypeDefinitionConfiguration: BaseEntityConfiguration<ActivityTypeDefinition>
    {
        public ActivityTypeDefinitionConfiguration()
        {
            this.HasRequired(x => x.PropertyType)
                .WithMany()
                .HasForeignKey(x => x.PropertyTypeId)
                .WillCascadeOnDelete(false);

            this.HasRequired(x => x.Country)
                .WithMany()
                .HasForeignKey(x => x.CountryId)
                .WillCascadeOnDelete(false);

            this.Property(x => x.Order).IsRequired();
        }
    }
}
