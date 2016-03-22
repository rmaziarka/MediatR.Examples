namespace KnightFrank.Antares.Dal.Model.Configuration
{
    using KnightFrank.Antares.Dal.Model;

    internal sealed class EnumLocalisedConfiguration : BaseEntityConfiguration<EnumLocalised>
    {
        public EnumLocalisedConfiguration()
        {
            this.Property(r => r.Value)
                .HasMaxLength(100);
        }
    }
}