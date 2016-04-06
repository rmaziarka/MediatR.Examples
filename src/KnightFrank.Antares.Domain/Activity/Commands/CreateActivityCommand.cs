namespace KnightFrank.Antares.Domain.Activity.Commands
{
    using System;
    using System.Collections.Generic;

    using MediatR;
    public class CreateActivityCommand : IRequest<Guid>
    {
        public Guid PropertyId { get; set; }
        
        public Guid ActivityStatusId { get; set; }

        public List<CreateActivityContact> Contacts { get; set; }
    }
}