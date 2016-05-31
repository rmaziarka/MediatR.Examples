namespace KnightFrank.Antares.Domain.LatestView.Commands
{
    using System;

    using KnightFrank.Antares.Dal.Model.Common;

    using MediatR;

    public class CreateLatestViewCommand : IRequest<Guid>
    {
        public EntityTypeEnum EntityType { get; set; }
        public Guid EntityId { get; set; }
    }
}
