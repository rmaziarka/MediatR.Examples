namespace KnightFrank.Antares.Dal.Model.Configuration.Property
{
    using KnightFrank.Antares.Dal.Model.Property;

    internal sealed class PropertyAreaBreakdownConfiguration : BaseEntityConfiguration<PropertyAreaBreakdown>
    {
        public PropertyAreaBreakdownConfiguration()
        {
            this.HasRequired(x => x.Property)
                .WithMany(x => x.PropertyAreaBreakdowns)
                .HasForeignKey(x => x.PropertyId)
                .WillCascadeOnDelete(false);

            this.Property(x => x.Name)
                .HasMaxLength(128)
                .IsRequired();
        }
    }
}