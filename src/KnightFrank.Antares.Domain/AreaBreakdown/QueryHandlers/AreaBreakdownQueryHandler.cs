namespace KnightFrank.Antares.Domain.AreaBreakdown.QueryHandlers
{
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Domain.AreaBreakdown.Queries;

    using MediatR;
    public class AreaBreakdownQueryHandler : IRequestHandler<AreaBreakdownQuery, IList<PropertyAreaBreakdown>>
    {
        public IList<PropertyAreaBreakdown> Handle(AreaBreakdownQuery message)
        {
            throw new System.NotImplementedException();
        }
    }
}