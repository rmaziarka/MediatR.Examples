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
                    .Include(v => v.Requirement.ChainTransactions)
                    .Include(v => v.Negotiator)
                    .Include(v => v.Activity)
                    .Include(v => v.Activity.ActivityDepartments)
                    .Include(v => v.Activity.ActivityDepartments.Select(ad => ad.Department))
                    .Include(v => v.Activity.Contacts)
                    .Include(v => v.Activity.AdvertisingPortals)
                    .Include(v => v.Activity.AppraisalMeetingAttendees)
                    .Include(v => v.Activity.ActivityUsers.Select(a => a.UserType))
                    .Include(v => v.Activity.ActivityUsers.Select(a => a.User))
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
                    .Include(v => v.Activity.ChainTransactions)
                    .Include(v => v.Activity.ChainTransactions.Select(c => c.AgentCompany))
                    .Include(v => v.Activity.ChainTransactions.Select(c => c.AgentContact))
                    .Include(v => v.Activity.ChainTransactions.Select(c => c.AgentUser))
                    .Include(v => v.Activity.ChainTransactions.Select(c => c.Property.Address))
                    .Include(v => v.Activity.ChainTransactions.Select(c => c.Property.Ownerships))
                    .Include(v => v.Activity.ChainTransactions.Select(c => c.SolicitorCompany))
                    .Include(v => v.Activity.ChainTransactions.Select(c => c.SolicitorContact))
                    .Include(v => v.Activity.ChainTransactions.Select(c => c.SolicitorContact))
                    .Include(v => v.Status)
                    .SingleOrDefault(v => v.Id == message.Id);

            return offer;
        }
    }
}
