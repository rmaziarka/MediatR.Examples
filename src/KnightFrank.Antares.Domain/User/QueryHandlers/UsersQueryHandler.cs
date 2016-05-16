namespace KnightFrank.Antares.Domain.User.QueryHandlers
{
    using System.Collections.Generic;
    using System.Linq;

    using Dal.Model.User;
    using Dal.Repository;
    using KnightFrank.Antares.Domain.User.Queries;
    using KnightFrank.Antares.Domain.User.QueryResults;

    using MediatR;

    public class UsersQueryHandler : IRequestHandler<UsersQuery, IEnumerable<UsersQueryResult>>
    {
        private readonly IReadGenericRepository<User> userRepository;

        public UsersQueryHandler(IReadGenericRepository<User> userRepository)
        {
            this.userRepository = userRepository;
        }

        public IEnumerable<UsersQueryResult> Handle(UsersQuery message)
        {
            if (string.IsNullOrWhiteSpace(message?.PartialName))
            {
                return new List<UsersQueryResult>();
            }

            string partialName = message.PartialName;

            IQueryable<User> userList = this.userRepository.Get();
            IOrderedQueryable<UsersQueryResult> orderedUserList = userList.Where(
                i =>
                    i.FirstName.StartsWith(partialName) ||
                    i.LastName.StartsWith(partialName)

                )
                .Select(x => new UsersQueryResult
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Department = x.Department.Name.ToString()
                })
                .OrderBy(x => x.FirstName).ThenBy(x => x.LastName);

            IQueryable<UsersQueryResult> usersQueryResults = 
                message.ExcludedIds != null ? orderedUserList.Where(u => message.ExcludedIds.All(e => e != u.Id)) : orderedUserList;

            if (message.Take > 0)
                return usersQueryResults.Take(message.Take);

            return usersQueryResults;
        }
    }
}
