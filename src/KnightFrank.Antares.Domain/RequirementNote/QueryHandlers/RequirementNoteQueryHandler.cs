namespace KnightFrank.Antares.Domain.RequirementNote.QueryHandlers
{
    using System.Data.Entity;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.RequirementNote.Queries;

    using MediatR;

    public class RequirementNoteQueryHandler : IRequestHandler<RequirementNoteQuery, RequirementNote>
    {
        private readonly IReadGenericRepository<RequirementNote> requirementNoteRepository;

        public RequirementNoteQueryHandler(IReadGenericRepository<RequirementNote> requirementNoteRepository)
        {
            this.requirementNoteRepository = requirementNoteRepository;
        }

        public RequirementNote Handle(RequirementNoteQuery message)
        {
            RequirementNote requirementNote =
                this.requirementNoteRepository
                    .Get()
                    .Include(rn => rn.Requirement)
                    .Include(rn => rn.User)
                    .SingleOrDefault(rn => rn.Id == message.Id);

            return requirementNote;
        }
    }
}
