namespace KnightFrank.Antares.Dal.Migrations
{
	using KnightFrank.Antares.Dal.Model;

    internal sealed class EnumTypeItemConfiguration : BaseEntityConfiguration<EnumTypeItem>
    {
        public EnumTypeItemConfiguration()
        {
            this.Property(r => r.Code)
                .HasMaxLength(40);
        }
    }
}