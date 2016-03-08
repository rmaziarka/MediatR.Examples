namespace KnightFrank.Antares.Dal.Migrations
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using KnightFrank.Antares.Dal.Model;

    internal sealed class EnumTypeItemConfiguration : BaseEntityConfiguration<EnumTypeItem>
    {
        public EnumTypeItemConfiguration()
        {
            this.HasRequired(r => r.EnumType);

            this.Property(r => r.Code)
                .HasMaxLength(40);
        }
    }
}