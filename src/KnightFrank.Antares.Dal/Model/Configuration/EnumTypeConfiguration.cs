namespace KnightFrank.Antares.Dal.Migrations
{
    using System.Data.Entity.ModelConfiguration;
    using KnightFrank.Antares.Dal.Model;

    internal sealed class EnumTypeConfiguration : EntityTypeConfiguration<EnumType>
    {
        public EnumTypeConfiguration()
        {
            this.Property(r => r.Code)
                .HasMaxLength(25);
        }
    }
}