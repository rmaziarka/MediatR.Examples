namespace KnightFrank.Antares.Dal.Model.Configuration.Enum
{
    using KnightFrank.Antares.Dal.Model.Enum;

    internal sealed class EnumLocalisedConfiguration : BaseEntityConfiguration<EnumLocalised>
    {
        public EnumLocalisedConfiguration()
        {
            this.Property(r => r.Value).HasMaxLength(100);
        }
    }
}
