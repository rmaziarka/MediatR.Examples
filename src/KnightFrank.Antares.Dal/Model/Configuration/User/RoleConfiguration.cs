namespace KnightFrank.Antares.Dal.Model.Configuration.User
{
    using KnightFrank.Antares.Dal.Model.User;

    internal sealed class RoleConfiguration : BaseEntityConfiguration<Role>
    {
        public RoleConfiguration()
        {
            this.Property(p => p.Name).HasMaxLength(100).IsRequired().IsUnique();

            this.HasMany(p => p.Users).WithMany()
                .Map(cs =>
                {
                    cs.MapLeftKey("RoleId");
                    cs.MapRightKey("UserId");
                    cs.ToTable("RoleUser");
                });
        }
    }
}
