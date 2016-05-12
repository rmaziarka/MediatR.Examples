namespace KnightFrank.Antares.Domain.Offer.QueryHandlers
{
    using System.Data.Entity;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Offer;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Offer.Queries;

    using MediatR;

    public class OfferQueryHandler : IRequestHandler<OfferQuery, Offer>
    {
        private readonly IReadGenericRepository<Offer> offerRepository;

        public OfferQueryHandler(IReadGenericRepository<Offer> offerRepository)
        {
            this.offerRepository = offerRepository;
        }

        public Offer Handle(OfferQuery message)
        {
            Offer offer =
                this.offerRepository
                    .Get()
                    .Include(v => v.Requirement)
                    .Include(v => v.Negotiator)
                    .Include(v => v.Activity)
                    .Include(v => v.Activity.Property.Address)
                    .SingleOrDefault(v => v.Id == message.Id);

            return offer;
        }
    }
}
