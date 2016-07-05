namespace KnightFrank.Antares.Dal.Model.Configuration.Property
{
    using KnightFrank.Antares.Dal.Model.Property;

    internal class RequirementTypeConfiguration : BaseEntityConfiguration<RequirementType>
    {
        public RequirementTypeConfiguration()
        {
            this.HasMany(x => x.ActivityTypeDefinitions)
                .WithMany(x=>x.RequirementTypes)
                .Map(cs =>
            {
                cs.MapLeftKey("RequirementTypeId");
                cs.MapRightKey("ActivityTypeDefinitionId");
            });

            this.Property(x => x.Code)
                .HasMaxLength(50)
                .IsRequired()
                .IsUnique();

            this.Property(x => x.EnumCode)
                .HasMaxLength(250)
                .IsRequired()
                .IsUnique();
        }
    }
}
