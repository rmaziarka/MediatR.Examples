namespace KnightFrank.Antares.Dal.Migrations
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
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