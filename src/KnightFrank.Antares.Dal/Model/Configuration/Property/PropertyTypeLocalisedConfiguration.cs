namespace KnightFrank.Antares.Dal.Model.Configuration.Property
{
    using KnightFrank.Antares.Dal.Model.Property;

    internal sealed class PropertyTypeLocalisedConfiguration : BaseEntityConfiguration<PropertyTypeLocalised>
    {
        public PropertyTypeLocalisedConfiguration()
        {
            this.HasRequired(x => x.Locale)
                .WithMany()
                .HasForeignKey(x => x.LocaleId)
                .WillCascadeOnDelete(false);

            this.Property(c => c.ResourceId).HasColumnName("PropertyTypeId");

            this.HasRequired(x => x.PropertyType)
                .WithMany()
                .HasForeignKey(x => x.ResourceId)
                .WillCascadeOnDelete(false);
            
            this.Property(x => x.Value)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
