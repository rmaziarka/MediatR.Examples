namespace KnightFrank.Antares.Dal.Model.Configuration
{
    using System.Data.Entity.ModelConfiguration;

    using KnightFrank.Antares.Dal.Model;

    internal class BaseAuditableEntityConfiguration<T> : EntityTypeConfiguration<T> where T : BaseAuditableEntity
    {
        public BaseAuditableEntityConfiguration()
        {
            // TODO: In future user should be required - for now it is optional not to brake for example activities
            this.HasOptional(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);
        }
    }
}