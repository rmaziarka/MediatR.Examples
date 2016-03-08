namespace KnightFrank.Antares.Dal.Migrations
{
    using System.Data.Entity.ModelConfiguration;
    using KnightFrank.Antares.Dal.Model;

    internal sealed class LocalConfiguration : EntityTypeConfiguration<Local>
    {
        public LocalConfiguration()
        {
            this.Property(r => r.IsoCode)
                .HasMaxLength(40);
        }
    }
}