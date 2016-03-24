namespace KnightFrank.Antares.Domain.Requirement.QueryHandlers
{
    using System.Data.Entity;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Requirement.Queries;

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
                    .FirstOrDefault(req => req.Id == message.Id);

            return requirement;
        }
    }
}
