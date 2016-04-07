namespace KnightFrank.Antares.Dal.Model.Configuration.Property
{
    using KnightFrank.Antares.Dal.Model.Attribute;

    internal class PropertyAttributeFormDefinitionConfiguration: BaseEntityConfiguration<PropertyAttributeFormDefinition>
    {
        public PropertyAttributeFormDefinitionConfiguration()
        {
            this.HasRequired(p => p.Attribute)
                .WithMany()
                .HasForeignKey(p => p.AttributeId);

            this.HasRequired(p => p.PropertyAttributeForm)
                .WithMany(p => p.PropertyAttributeFormDefinitions)
                .HasForeignKey(p => p.PropertyAttributeFormId);
        }
    }
}
