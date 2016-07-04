namespace KnightFrank.Antares.Domain.User.Commands
{
    using System;

    using MediatR;

    public class UpdateUserCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }

        public Guid SalutationFormatId { get; set; }

     }
}
