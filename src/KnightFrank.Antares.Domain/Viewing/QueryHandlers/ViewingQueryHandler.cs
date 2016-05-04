namespace KnightFrank.Antares.Domain.Viewing.QueryHandlers
{
    using System.Data.Entity;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Viewing.Queries;

    using MediatR;

    public class ViewingQueryHandler : IRequestHandler<ViewingQuery, Viewing>
    {
        private readonly IReadGenericRepository<Viewing> viewingRepository;

        public ViewingQueryHandler(IReadGenericRepository<Viewing> viewingRepository)
        {
            this.viewingRepository = viewingRepository;
        }

        public Viewing Handle(ViewingQuery message)
        {
            Viewing viewing =
                this.viewingRepository
                    .Get()
                    .Include(v => v.Attendees)
                    .Include(v => v.Requirement)
                    .Include(v => v.Activity)
                    .Include(v => v.Activity.Property.Address)
                    .SingleOrDefault(v => v.Id == message.Id);

            return viewing;
        }
    }
}
