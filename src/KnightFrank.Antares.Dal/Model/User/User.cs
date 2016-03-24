namespace KnightFrank.Antares.Dal.Model.User
{
    using System;
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Resource;

    public class User : BaseEntity
    {
        public string ActiveDirectoryDomain { get; set; }

        public string ActiveDirectoryLogin { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Guid BusinessId { get; set; }

        public virtual Business Business { get; set; }

        public Guid CountryId { get; set; }

        public virtual Country Country { get; set; }

        public Guid DepartmentId { get; set; }

        public virtual Department Department { get; set; }

        public Guid LocaleId { get; set; }

        public virtual Locale Locale { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }
}
