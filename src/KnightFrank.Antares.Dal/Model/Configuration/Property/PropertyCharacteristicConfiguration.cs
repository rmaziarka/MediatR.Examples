namespace KnightFrank.Antares.Dal.Model.Configuration.Property
{
    using KnightFrank.Antares.Dal.Model.Property;

    internal sealed class PropertyCharacteristicConfiguration : BaseEntityConfiguration<PropertyCharacteristic>
    {
        public PropertyCharacteristicConfiguration()
        {
            this.HasRequired(p => p.Property).WithMany().HasForeignKey(p => p.PropertyId).WillCascadeOnDelete(false);
            this.HasRequired(p => p.Characteristic).WithMany().HasForeignKey(p => p.CharacteristicId).WillCascadeOnDelete(false);
        }
    }
}