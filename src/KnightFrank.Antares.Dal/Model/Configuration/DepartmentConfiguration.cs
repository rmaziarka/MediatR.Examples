namespace KnightFrank.Antares.Dal.Model.Configuration
{
	using KnightFrank.Antares.Dal.Migrations;

	internal sealed class DepartmentConfiguration : BaseEntityConfiguration<Department>
	{
		public DepartmentConfiguration()
		{
			this.Property(p => p.Name).HasMaxLength(255);
		}
	}
}
