namespace KnightFrank.Antares.Domain.Tenancy.QueryHandlers
{
    using System.Data.Entity;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Tenancy;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Tenancy.Queries;

    using MediatR;

    public class TenancyQueryHandler : IRequestHandler<TenancyQuery, Tenancy>
    {
        private readonly IReadGenericRepository<Tenancy> tenancyRepository;

        public TenancyQueryHandler(IReadGenericRepository<Tenancy> tenancyRepository)
        {
            this.tenancyRepository = tenancyRepository;
        }

        public Tenancy Handle(TenancyQuery message)
        {
            return this.tenancyRepository.Get()
                       .Include(x => x.Requirement)
                       .Include(x => x.Activity)
                       .Include(x => x.Terms)
                       .Include(x => x.Contacts)
                       .Include(x => x.Contacts.Select(c => c.Contact))
                       .SingleOrDefault(x => x.Id == message.Id);
        }
    }
}