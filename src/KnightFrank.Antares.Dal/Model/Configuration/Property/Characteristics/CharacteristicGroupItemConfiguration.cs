namespace KnightFrank.Antares.Dal.Model.Configuration.Property.Characteristics
{
    using KnightFrank.Antares.Dal.Model.Property.Characteristics;
    internal sealed class CharacteristicGroupItemConfiguration : BaseEntityConfiguration<CharacteristicGroupItem>
    {
        public CharacteristicGroupItemConfiguration()
        {
            this.HasRequired(p => p.Characteristic).WithMany().HasForeignKey(p => p.CharacteristicId).WillCascadeOnDelete(false);

            this.HasRequired(p => p.CharacteristicGroupUsage)
                .WithMany(p => p.CharacteristicGroupItems)
                .HasForeignKey(p => p.CharacteristicGroupUsageId)
                .WillCascadeOnDelete(false);
        }
    }
}