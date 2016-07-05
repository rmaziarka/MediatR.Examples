namespace KnightFrank.Antares.Domain.Offer.Queries
{
    using System.Collections.Generic;

    using KnightFrank.Antares.Domain.Offer.QueryResults;

    using MediatR;

    public class OfferTypeQuery : IRequest<IList<OfferTypeQueryResult>>
    {
    }
}
