namespace KnightFrank.Antares.Dal.Model.Configuration
{
    using KnightFrank.Antares.Dal.Model;

    internal sealed class EnumTypeConfiguration : BaseEntityConfiguration<EnumType>
    {
        public EnumTypeConfiguration()
        {
            this.Property(r => r.Code)
                .HasMaxLength(25);
        }
    }
}