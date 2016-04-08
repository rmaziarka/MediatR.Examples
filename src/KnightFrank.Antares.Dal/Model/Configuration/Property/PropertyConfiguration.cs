namespace KnightFrank.Antares.Dal.Model.Configuration.Property
{
    internal class PropertyConfiguration : BaseEntityConfiguration<Model.Property.Property>
    {
        public PropertyConfiguration()
        {
            this.HasRequired(p => p.Address).WithMany().HasForeignKey(p => p.AddressId);

            this.HasRequired(p => p.PropertyType);

            this.HasOptional(p => p.AttributeValues).WithMany().HasForeignKey(p => p.AttributeValuesId);
        }
    }
}
