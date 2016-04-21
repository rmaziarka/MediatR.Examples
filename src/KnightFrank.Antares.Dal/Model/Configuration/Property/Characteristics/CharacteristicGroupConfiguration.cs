namespace KnightFrank.Antares.Dal.Model.Configuration.Property.Characteristics
{
    using KnightFrank.Antares.Dal.Model.Property.Characteristics;

    internal sealed class CharacteristicGroupConfiguration : BaseEntityConfiguration<CharacteristicGroup>
    {
        public CharacteristicGroupConfiguration()
        {
            this.Property(p => p.Code).HasMaxLength(50);
        }
    }
}
