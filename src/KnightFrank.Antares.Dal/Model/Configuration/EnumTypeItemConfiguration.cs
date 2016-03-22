namespace KnightFrank.Antares.Dal.Model.Configuration
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