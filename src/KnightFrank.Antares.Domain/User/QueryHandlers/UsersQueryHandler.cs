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
            return this.userRepository.Get()
                       .Where(
                           i =>
                               i.FirstName.StartsWith(message.PartialName) ||
                               i.LastName.StartsWith(message.PartialName))
                       .Select(x => new UsersQueryResult
                       {
                           FirstName = x.FirstName,
                           LastName = x.LastName,
                           Department = x.Department.Name.ToString()
                       }).OrderBy(x=>x.FirstName).ThenBy(x=>x.LastName).ToList();

        }
    }
}
