namespace KnightFrank.Antares.Dal.Model.Configuration.PropertyType
{
    using KnightFrank.Antares.Dal.Model.PropertyType;
    internal sealed class PropertyTypeLocalisedConfiguration : BaseEntityConfiguration<PropertyTypeLocalised>
    {
        public PropertyTypeLocalisedConfiguration()
        {
            this.HasRequired(x => x.Locale)
                .WithMany()
                .HasForeignKey(x => x.LocaleId)
                .WillCascadeOnDelete(false);

            this.HasRequired(x => x.PropertyType)
                .WithMany()
                .HasForeignKey(x => x.PropertyTypeId)
                .WillCascadeOnDelete(false);

            this.Property(x => x.Value)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
