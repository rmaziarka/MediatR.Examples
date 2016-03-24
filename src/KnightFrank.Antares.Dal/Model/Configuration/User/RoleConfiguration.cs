namespace KnightFrank.Antares.Dal.Model.Configuration.User
{
    using KnightFrank.Antares.Dal.Model.User;

    internal sealed class RoleConfiguration : BaseEntityConfiguration<Role>
    {
        public RoleConfiguration()
        {
            this.Property(p => p.Name).HasMaxLength(100);

            this.HasMany(p => p.Users).WithMany(p => p.Roles);
        }
    }
}
