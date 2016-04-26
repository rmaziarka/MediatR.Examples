namespace KnightFrank.Antares.Domain.Activity.Queries
{
    using System.Collections.Generic;

    using KnightFrank.Antares.Domain.Activity.QueryResults;

    using MediatR;

    public class ActivitiesQuery : IRequest<IEnumerable<ActivitiesQueryResult>>
    {
    }
}
