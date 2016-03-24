namespace KnightFrank.Antares.Dal.Model.Configuration.Property
{
    using KnightFrank.Antares.Dal.Model.Property;

    internal sealed class PropertyTypeDefinitionConfiguration : BaseEntityConfiguration<PropertyTypeDefinition>
    {
        public PropertyTypeDefinitionConfiguration()
        {
            this.HasRequired(x => x.PropertyType)
                .WithMany()
                .HasForeignKey(x => x.PropertyTypeId)
                .WillCascadeOnDelete(false);

            this.HasRequired(x => x.Country)
                .WithMany()
                .HasForeignKey(x => x.CountryId)
                .WillCascadeOnDelete(false);

            this.HasRequired(x => x.Division)
                .WithMany()
                .HasForeignKey(x => x.DivisionId)
                .WillCascadeOnDelete(false);

            this.Property(x => x.Order).IsRequired();
        }
    }
}
