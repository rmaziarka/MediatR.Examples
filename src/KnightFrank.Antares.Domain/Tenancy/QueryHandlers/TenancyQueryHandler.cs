namespace KnightFrank.Antares.Domain.Tenancy.QueryHandlers
{
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
            return this.tenancyRepository.GetWithInclude(x => x.Id == message.Id,
                x => x.Contacts,
                x => x.Requirement,
                x => x.Activity,
                x => x.Terms).FirstOrDefault();
        }
    }
}