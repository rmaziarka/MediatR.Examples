namespace KnightFrank.Antares.Domain.Offer.Queries
{
    using System;

    using KnightFrank.Antares.Dal.Model.Offer;

    using MediatR;

    public class OfferQuery : IRequest<Offer>
    {
        public Guid Id { get; set; }
    }
}
