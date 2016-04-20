namespace KnightFrank.Antares.Dal.Model.Configuration.Property.Characteristics
{
    using KnightFrank.Antares.Dal.Model.Property.Characteristics;
    internal sealed class CharacteristicGroupLocalizedConfiguration : BaseEntityConfiguration<CharacteristicGroupLocalised>
    {
        public CharacteristicGroupLocalizedConfiguration()
        {
            this.HasRequired(p => p.Locale).WithMany().HasForeignKey(p => p.LocaleId).WillCascadeOnDelete(false);
            this.HasRequired(p => p.CharacteristicGroup)
                .WithMany()
                .HasForeignKey(p => p.ResourceId)
                .WillCascadeOnDelete(false);

            this.Property(c => c.ResourceId).HasColumnName("CharacteristicGroupId");

            this.Property(p => p.Value).HasMaxLength(100);
        }
    }
}