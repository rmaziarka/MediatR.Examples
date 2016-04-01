namespace KnightFrank.Antares.Domain.Activity.Commands
{
    using System;

    using MediatR;
    public class CreateActivityCommand : IRequest<Guid>
    {
        public Guid PropertyId { get; set; }
        
        public Guid ActivityStatusId { get; set; }
    }
}