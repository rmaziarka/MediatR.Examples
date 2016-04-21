namespace KnightFrank.Antares.Dal.Model.Configuration.Property.Characteristics
{
    using KnightFrank.Antares.Dal.Model.Property.Characteristics;

    internal sealed class CharacteristicConfiguration : BaseEntityConfiguration<Characteristic>
    {
        public CharacteristicConfiguration()
        {
            this.Property(p => p.Code).HasMaxLength(50);
        }
    }
}
