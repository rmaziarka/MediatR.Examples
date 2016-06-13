namespace KnightFrank.Antares.Dal.Model.Configuration.User
{
    using KnightFrank.Antares.Dal.Model.User;

    internal sealed class UserConfiguration : BaseEntityConfiguration<User>
    {
        public UserConfiguration()
        {
            this.Property(p => p.ActiveDirectoryDomain).HasMaxLength(40);

            this.Property(p => p.ActiveDirectoryLogin).HasMaxLength(100).IsRequired().IsUnique();

            this.Property(p => p.FirstName).HasMaxLength(40);

            this.Property(p => p.LastName).HasMaxLength(40);

            this.HasMany(p => p.Roles).WithMany(p => p.Users);

            this.HasOptional(x => x.SalutationFormat)
             .WithMany()
             .HasForeignKey(x => x.SalutationFormatId)
             .WillCascadeOnDelete(false);
        }
    }
}
