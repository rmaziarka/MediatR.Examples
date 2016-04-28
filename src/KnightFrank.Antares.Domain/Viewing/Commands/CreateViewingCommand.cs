namespace KnightFrank.Antares.Domain.Viewing.Commands
{
    using System;
    using System.Collections.Generic;

    using MediatR;

    public class CreateViewingCommand : IRequest<Guid>
    {
        public CreateViewingCommand()
        {
            this.AttendeesIds = new List<Guid>();
        }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        
        public string InvitationText { get; set; }

        public string PostViewingComment { get; set; }

        public IList<Guid> AttendeesIds { get; set; }

        public Guid RequirementId { get; set; }

        public Guid ActivityId { get; set; }

        public Guid NegotiatorId { get; set; }
    }
}