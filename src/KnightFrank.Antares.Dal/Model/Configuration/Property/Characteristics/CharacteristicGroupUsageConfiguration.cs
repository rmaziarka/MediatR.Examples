namespace KnightFrank.Antares.Dal.Model.Configuration.Property.Characteristics
{
    using KnightFrank.Antares.Dal.Model.Property.Characteristics;
    internal sealed class CharacteristicGroupUsageConfiguration : BaseEntityConfiguration<CharacteristicGroupUsage>
    {
        public CharacteristicGroupUsageConfiguration()
        {
            this.HasRequired(p => p.PropertyType).WithMany().HasForeignKey(p => p.PropertyTypeId).WillCascadeOnDelete(false);
            this.HasRequired(p => p.CharacteristicGroup)
                .WithMany()
                .HasForeignKey(p => p.CharacteristicGroupId)
                .WillCascadeOnDelete(false);
            this.HasRequired(p => p.Country).WithMany().HasForeignKey(p => p.CountryId).WillCascadeOnDelete(false);
        }
    }
}