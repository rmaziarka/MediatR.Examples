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
                    .Include(v => v.Requirement.Contacts)
                    .Include(v => v.Requirement.Solicitor)
                    .Include(v => v.Requirement.SolicitorCompany)
                    .Include(v => v.Negotiator)
                    .Include(v => v.Activity)
                    .Include(v => v.Activity.Contacts)
                    .Include(v => v.Activity.Property.Address)
                    .Include(v => v.Activity.Solicitor)
                    .Include(v => v.Activity.SolicitorCompany)
                    .Include(v => v.Broker)
                    .Include(v => v.Lender)
                    .Include(v => v.Surveyor)
                    .Include(v => v.AdditionalSurveyor)
                    .Include(v => v.BrokerCompany)
                    .Include(v => v.LenderCompany)
                    .Include(v => v.SurveyorCompany)
                    .Include(v => v.AdditionalSurveyorCompany)
                    .SingleOrDefault(v => v.Id == message.Id);

            return offer;
        }
    }
}
