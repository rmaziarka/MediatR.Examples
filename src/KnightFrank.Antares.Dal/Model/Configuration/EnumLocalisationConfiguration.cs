namespace KnightFrank.Antares.Dal.Migrations
{
    using System.Data.Entity.ModelConfiguration;
    using KnightFrank.Antares.Dal.Model;

    internal sealed class EnumLocalisationConfiguration : EntityTypeConfiguration<EnumLocalisation>
    {
        public EnumLocalisationConfiguration()
        {
            this.HasRequired(r => r.Local);
            this.HasRequired(r => r.EnumTypeItem);

            this.Property(r => r.Value)
                .HasMaxLength(100);
        }
    }
}