namespace KnightFrank.Antares.Domain.Offer.QueryHandlers
{
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Offer;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Offer.Queries;
    using KnightFrank.Antares.Domain.Offer.QueryResults;

    using MediatR;

    public class OfferTypeQueryHandler : IRequestHandler<OfferTypeQuery, IList<OfferTypeQueryResult>>
    {
        private readonly IReadGenericRepository<OfferType> offerTypeRepository;

        public OfferTypeQueryHandler(IReadGenericRepository<OfferType> offerTypeRepository)
        {
            this.offerTypeRepository = offerTypeRepository;
        }

        public IList<OfferTypeQueryResult> Handle(OfferTypeQuery message)
        {
            IList<OfferTypeQueryResult> offerTypes =
                this.offerTypeRepository.Get()
                    .Select(x => new OfferTypeQueryResult
                    {
                        Id = x.Id,
                        EnumCode = x.EnumCode
                    })
                    .ToList();

            return offerTypes;
        }
    }
}
