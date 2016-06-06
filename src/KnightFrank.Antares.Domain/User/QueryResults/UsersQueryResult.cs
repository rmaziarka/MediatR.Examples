namespace KnightFrank.Antares.Domain.User.QueryResults
{
    using System;

    using KnightFrank.Antares.Dal.Model.User;

    public class UsersQueryResult
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Department Department { get; set; }

    }
}