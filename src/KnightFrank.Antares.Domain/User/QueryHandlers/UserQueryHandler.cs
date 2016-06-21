namespace KnightFrank.Antares.Domain.User.QueryHandlers
{
    using System.Linq;

    using Dal.Model.User;
    using Dal.Repository;
    using KnightFrank.Antares.Domain.User.Queries;

    using MediatR;

    public class UserQueryHandler : IRequestHandler<UserQuery, User>
    {
        private readonly IReadGenericRepository<User> userRepository;

        public UserQueryHandler(IReadGenericRepository<User> userRepository)
        {
            this.userRepository = userRepository;
        }

        public User Handle(UserQuery query)
        {
            return this.userRepository.Get()
                 .SingleOrDefault(a => a.Id == query.Id);
        }
    }
}
