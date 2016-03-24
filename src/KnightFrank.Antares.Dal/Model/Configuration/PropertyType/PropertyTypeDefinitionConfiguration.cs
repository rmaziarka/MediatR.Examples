namespace KnightFrank.Antares.Dal.Model.Configuration.PropertyType
{
    using KnightFrank.Antares.Dal.Model.PropertyType;
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
        }
    }
}
