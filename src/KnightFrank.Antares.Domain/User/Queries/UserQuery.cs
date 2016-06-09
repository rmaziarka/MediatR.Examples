namespace KnightFrank.Antares.Domain.User.Queries
{
    using System;

    using KnightFrank.Antares.Dal.Model.User;

    using MediatR;

    public class UserQuery : IRequest<User>
    {
        public Guid Id { get; set; }
    }
}

