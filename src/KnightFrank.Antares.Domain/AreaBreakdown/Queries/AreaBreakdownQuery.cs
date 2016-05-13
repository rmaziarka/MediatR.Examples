namespace KnightFrank.Antares.Domain.AreaBreakdown.Queries
{
    using System;
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Property;

    using MediatR;

    public class AreaBreakdownQuery : IRequest<IList<PropertyAreaBreakdown>>
    {
        public Guid PropertyId { get; set; }

        public IList<Guid> AreaIds { get; set; } 
    }
}