namespace KnightFrank.Antares.Domain.User.Queries
{
    using System;
    using System.Collections.Generic;

    using User.QueryResults;

    using MediatR;

    public class UsersQuery : IRequest<IEnumerable<UsersQueryResult>>
    {
        public string PartialName { get; set; }
        public int Take { get; set; }
        public IEnumerable<Guid> ExcludedIds { get; set; }
    }
}
