namespace KnightFrank.Antares.Domain.User.QueryResults
{
    using System;

    public class UsersQueryResult
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Department { get; set; }

    }
}