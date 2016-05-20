namespace KnightFrank.Antares.Domain.AreaBreakdown.Commands
{
    using System;

    using MediatR;

    public class UpdateAreaBreakdownOrderCommand : IRequest<Guid>
    {
        public Guid PropertyId { get; set; }

        public Guid AreaId { get; set; }
        
        public int Order { get; set; }
    }
}