namespace KnightFrank.Antares.Domain.Activity.Commands
{
    using System;
    using System.Collections.Generic;

    using MediatR;
    public class CreateActivityCommand : IRequest<Guid>
    {
        public CreateActivityCommand()
        {
            this.ContactIds = new List<Guid>();
        }

        public Guid PropertyId { get; set; }

        public Guid ActivityStatusId { get; set; }

        public IList<Guid> ContactIds { get; set; }

        public Guid ActivityTypeId { get; set; }

        public Guid LeadNegotiatorId { get; set; }
    }
}