namespace KnightFrank.Antares.Domain.AreaBreakdown.Commands
{
    using System;
    using System.Collections.Generic;

    using MediatR;

    public class CreateAreaBreakdownCommand : IRequest<IList<Guid>>
    {
        public Guid PropertyId { get; set; }

        public IList<Area> Areas { get; set; }
    }
}