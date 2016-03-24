namespace KnightFrank.Antares.Dal.Model.Configuration.User
{
    using KnightFrank.Antares.Dal.Model.User;

    internal sealed class DepartmentConfiguration : BaseEntityConfiguration<Department>
	{
		public DepartmentConfiguration()
		{
			this.Property(p => p.Name).HasMaxLength(255);

			this.HasMany(p => p.Users)
				.WithRequired(p => p.Department)
				.WillCascadeOnDelete(false);
		}
	}
}
