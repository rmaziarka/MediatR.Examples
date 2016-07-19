namespace KnightFrank.Antares.Domain.Tenancy.Queries
{
    using System.Collections.Generic;

    using KnightFrank.Antares.Domain.Tenancy.QueryResults;

    using MediatR;

    public class TenancyTypeQuery : IRequest<IList<TenancyTypeQueryResult>>
    {
    }
}
