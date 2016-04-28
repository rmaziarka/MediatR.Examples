namespace KnightFrank.Antares.Domain.Activity.Queries
{
    using System;
    using System.Collections.Generic;

    using KnightFrank.Antares.Domain.Activity.QueryResults;

    using MediatR;

    public class ActivitiesQuery : IRequest<IEnumerable<ActivitiesQueryResult>>
    {
        public Guid RequirementId { get; set; }
    }
}
