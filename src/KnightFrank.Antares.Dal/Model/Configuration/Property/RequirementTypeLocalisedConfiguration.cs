namespace KnightFrank.Antares.Dal.Model.Configuration.Property
{
    using KnightFrank.Antares.Dal.Model.Property;

    internal class RequirementTypeLocalisedConfiguration : BaseEntityConfiguration<RequirementTypeLocalised>
    {
        public RequirementTypeLocalisedConfiguration()
        {
            this.HasRequired(x => x.Locale)
                .WithMany()
                .HasForeignKey(x => x.LocaleId)
                .WillCascadeOnDelete(false);

            this.Property(c => c.ResourceId).HasColumnName("RequirementTypeId");

            this.HasRequired(x => x.RequirementType)
                .WithMany()
                .HasForeignKey(x => x.ResourceId)
                .WillCascadeOnDelete(false);

            this.Property(x => x.Value)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}