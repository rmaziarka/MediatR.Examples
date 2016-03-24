namespace KnightFrank.Antares.Dal.Model.User
{
    using System;
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Resource;

    public class Department : BaseEntity
	{
		public string Name { get; set; }

		public Guid CountryId { get; set; }

		public virtual Country Country { get; set; }

		public virtual ICollection<User> Users { get; set; }
	}
}
