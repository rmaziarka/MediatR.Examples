namespace KnightFrank.Antares.Dal.Model.Configuration.User
{
    using KnightFrank.Antares.Dal.Model.User;

    internal sealed class BusinessConfiguration : BaseEntityConfiguration<Business>
    {
        public BusinessConfiguration()
        {
            this.Property(p => p.Name).HasMaxLength(100).IsRequired().IsUnique();

            this.HasMany(p => p.Users)
                .WithRequired(p => p.Business)
                .WillCascadeOnDelete(false);
        }
    }
}
