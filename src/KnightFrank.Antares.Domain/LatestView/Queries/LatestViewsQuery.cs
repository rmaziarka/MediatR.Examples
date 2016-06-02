namespace KnightFrank.Antares.Domain.LatestView.Queries
{
    using System.Collections.Generic;

    using KnightFrank.Antares.Domain.LatestView.QueryResults;

    using MediatR;

    public class LatestViewsQuery : IRequest<IEnumerable<LatestViewQueryResultItem>>
    {
    }
}
