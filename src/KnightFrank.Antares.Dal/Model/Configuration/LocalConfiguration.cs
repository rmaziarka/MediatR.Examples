namespace KnightFrank.Antares.Dal.Migrations
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using KnightFrank.Antares.Dal.Model;

    internal sealed class LocalConfiguration : BaseEntityConfiguration<Local>
    {
        public LocalConfiguration()
        {
            this.Property(r => r.IsoCode)
                .HasMaxLength(40);
        }
    }
}