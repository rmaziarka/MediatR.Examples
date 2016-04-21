namespace KnightFrank.Antares.Dal.Model.Configuration.Property.Characteristics
{
    using KnightFrank.Antares.Dal.Model.Property.Characteristics;

    internal sealed class CharacteristicGroupLocalisedConfiguration : BaseEntityConfiguration<CharacteristicGroupLocalised>
    {
        public CharacteristicGroupLocalisedConfiguration()
        {
            this.HasRequired(x => x.Locale)
                .WithMany()
                .HasForeignKey(x => x.LocaleId)
                .WillCascadeOnDelete(false);

            this.HasRequired(x => x.CharacteristicGroup)
                .WithMany()
                .HasForeignKey(x => x.ResourceId)
                .WillCascadeOnDelete(false);

            this.Property(c => c.ResourceId).HasColumnName("CharacteristicGroupId");

            this.Property(p => p.Value).HasMaxLength(100).IsRequired();
        }
    }
}
