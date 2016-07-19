namespace KnightFrank.Antares.Domain.Requirement.QueryHandlers
{
    using System.Data.Entity;
    using System.Linq;

    using Dal.Model.Property;
    using Dal.Repository;
    using Queries;

    using MediatR;

    public class RequirementQueryHandler : IRequestHandler<RequirementQuery, Requirement>
    {
        private readonly IReadGenericRepository<Requirement> requirementRepository;

        public RequirementQueryHandler(IReadGenericRepository<Requirement> requirementRepository)
        {
            this.requirementRepository = requirementRepository;
        }

        public Requirement Handle(RequirementQuery message)
        {
            Requirement requirement =
                this.requirementRepository
                    .Get()
                    .Include(req => req.Contacts)
                    .Include(req => req.Address)
                    .Include(req => req.RequirementNotes)
                    .Include(req => req.RequirementType)
                    .Include(req => req.Viewings)
                    .Include(req => req.Viewings.Select(v => v.Attendees))
                    .Include(req => req.Viewings.Select(v => v.Negotiator))
                    .Include(req => req.Viewings.Select(v => v.Activity.Property.Address))
                    .Include(req => req.RequirementNotes.Select(rn => rn.User))
                    .Include(req => req.Offers)
                    .Include(req => req.Offers.Select(v => v.OfferType))
                    .Include(req => req.Offers.Select(v => v.Status))
                    .Include(req => req.Offers.Select(v => v.Negotiator))
                    .Include(req => req.Offers.Select(v => v.Activity.Property.Address))
                    .Include(req => req.Attachments)
                    .Include(req => req.Attachments.Select(at => at.User))
                    .Include(req => req.Tenancy.Terms)
                    .Include(req => req.Tenancy.Activity.Property.Address)
                    .SingleOrDefault(req => req.Id == message.Id);

            return requirement;
        }
    }
}
