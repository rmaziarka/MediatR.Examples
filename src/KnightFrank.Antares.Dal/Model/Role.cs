namespace KnightFrank.Antares.Dal.Model
{
	using System.Collections.Generic;

	public class Role : BaseEntity
	{
		public string Name { get; set; }

		public virtual ICollection<User> Users { get; set; } 
	}
}
