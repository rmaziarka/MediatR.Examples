namespace KnightFrank.Antares.Dal.Model.Configuration
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using KnightFrank.Antares.Dal.Model;

    internal class BaseEntityConfiguration<T> : EntityTypeConfiguration<T> where T : BaseEntity
    {
        public BaseEntityConfiguration()
        {
            this.Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}