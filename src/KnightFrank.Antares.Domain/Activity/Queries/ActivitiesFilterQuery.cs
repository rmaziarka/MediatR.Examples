namespace KnightFrank.Antares.Domain.Activity.Queries
{
    using System;
    using System.Collections.Generic;
    
    using KnightFrank.Antares.Domain.Activity.QueryResults;

    using MediatR;

    public class ActivitiesFilterQuery : IRequest<IEnumerable<ActivitiesQueryResult>>
    {
        public string CountryCode { get; set; }
        public Guid RequirementTypeId { get; set; }
    }
}
