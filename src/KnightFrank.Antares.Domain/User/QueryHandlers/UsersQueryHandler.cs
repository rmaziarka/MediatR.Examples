namespace KnightFrank.Antares.Domain.User.QueryHandlers
{
    using System;
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
            if (!string.IsNullOrWhiteSpace(message?.PartialName))
            {

                IQueryable<User> userList = this.userRepository.Get();
                return userList.Where(
                    i =>
                        i.FirstName.ToLower().StartsWith(message.PartialName.ToLower()) ||
                        i.LastName.ToLower().StartsWith(message.PartialName.ToLower()))
                               .Select(x => new UsersQueryResult
                               {
                                   Id = x.Id,
                                   FirstName = x.FirstName,
                                   LastName = x.LastName,
                                   Department = x.Department.Name.ToString()
                               }).OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();
            }

            return new List<UsersQueryResult>();

        }
    }
}
