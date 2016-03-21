namespace KnightFrank.Antares.Dal.Model.Configuration
{
	using KnightFrank.Antares.Dal.Migrations;

	internal sealed class UserConfiguration : BaseEntityConfiguration<User>
	{
		public UserConfiguration()
		{
			this.Property(p => p.ActiveDirectoryDomain).HasMaxLength(40);

			this.Property(p => p.ActiveDirectoryLogin).HasMaxLength(100);

			this.Property(p => p.FirstName).HasMaxLength(40);

			this.Property(p => p.LastName).HasMaxLength(40);

			this.HasMany(p => p.Roles).WithMany(p => p.Users);
		}
	}
}
