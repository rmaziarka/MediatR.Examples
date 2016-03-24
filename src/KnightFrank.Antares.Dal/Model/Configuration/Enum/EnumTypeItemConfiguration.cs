namespace KnightFrank.Antares.Dal.Model.Configuration.Enum
{
    using KnightFrank.Antares.Dal.Model.Enum;

    internal sealed class EnumTypeItemConfiguration : BaseEntityConfiguration<EnumTypeItem>
    {
        public EnumTypeItemConfiguration()
        {
            this.Property(r => r.Code).HasMaxLength(40);
        }
    }
}
