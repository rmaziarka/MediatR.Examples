namespace KnightFrank.Antares.Domain.AreaBreakdown.Commands
{
    using System;

    using MediatR;

    public class UpdateAreaBreakdownCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }

        public Guid PropertyId { get; set; }

        public string Name { get; set; }

        public double Size { get; set; }
    }
}
