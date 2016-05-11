namespace KnightFrank.Antares.Domain.Viewing.Commands
{
    using System;
    using System.Collections.Generic;

    using MediatR;

    public class UpdateViewingCommand : IRequest<Guid>
    {
        public UpdateViewingCommand()
        {
            this.AttendeesIds = new List<Guid>();
        }

        public Guid Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        
        public string InvitationText { get; set; }

        public string PostViewingComment { get; set; }

        public IList<Guid> AttendeesIds { get; set; }
    }
}