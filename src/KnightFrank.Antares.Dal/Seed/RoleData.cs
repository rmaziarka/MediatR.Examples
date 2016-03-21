namespace KnightFrank.Antares.Dal.Seed
{
	using System.Data.Entity.Migrations;

	using KnightFrank.Antares.Dal.Model;

	internal class RoleData
	{
		public static void Seed(KnightFrankContext context)
		{
			SeedRole(context);
		}

		public static void SeedRole(KnightFrankContext context)
		{
			SeedRoleData(context, "System Administrator");
			SeedRoleData(context, "Residential User");
			SeedRoleData(context, "Commercial User");
		}

		private static void SeedRoleData(KnightFrankContext context, string name)
		{
			var role = new Role
			{
				Name = name
			};

			context.Role.AddOrUpdate(x => x.Name, role);
			context.SaveChanges();
		}
	}
}
