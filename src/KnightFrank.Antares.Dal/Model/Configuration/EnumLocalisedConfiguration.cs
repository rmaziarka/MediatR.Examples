namespace KnightFrank.Antares.Dal.Migrations
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