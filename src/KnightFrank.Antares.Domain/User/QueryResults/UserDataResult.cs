namespace KnightFrank.Antares.Domain.User.QueryResults
{
    using System;
    using System.Collections.Generic;

    public class UserDataResult
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Country { get; set; }

        public Guid Division { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}
