namespace KnightFrank.Antares.Domain.Activity.Commands
{
    using System;

    using MediatR;

    public class UpdateActivityUserCommand : IRequest<Guid>
    {
        public Guid UserId { get; set; }

        public DateTime? CallDate { get; set; }
    }
}