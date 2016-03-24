namespace KnightFrank.Antares.Dal.Model.Configuration.Enum
{
    using KnightFrank.Antares.Dal.Model.Enum;

    internal sealed class EnumTypeConfiguration : BaseEntityConfiguration<EnumType>
    {
        public EnumTypeConfiguration()
        {
            this.Property(r => r.Code).HasMaxLength(25);
        }
    }
}
