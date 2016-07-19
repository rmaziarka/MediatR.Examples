namespace KnightFrank.Antares.Dal.Model.Configuration.Tenancy
{
    using KnightFrank.Antares.Dal.Model.Tenancy;
    internal sealed class TenancyTypeConfiguration : BaseEntityConfiguration<TenancyType>
    {
        public TenancyTypeConfiguration()
        {
            this.Property(x => x.Code)
                .HasMaxLength(50)
                .IsRequired();

            this.Property(x => x.EnumCode)
                .HasMaxLength(250)
                .IsRequired()
                .IsUnique();
        }
    }
}
