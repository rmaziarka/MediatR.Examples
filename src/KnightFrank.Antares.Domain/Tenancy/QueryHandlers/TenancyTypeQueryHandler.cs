namespace KnightFrank.Antares.Domain.Tenancy.QueryHandlers
{
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Tenancy;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Tenancy.Queries;
    using KnightFrank.Antares.Domain.Tenancy.QueryResults;

    using MediatR;

    public class TenancyTypeQueryHandler : IRequestHandler<TenancyTypeQuery, IList<TenancyTypeQueryResult>>
    {
        private readonly IReadGenericRepository<TenancyType> offerTypeRepository;

        public TenancyTypeQueryHandler(IReadGenericRepository<TenancyType> offerTypeRepository)
        {
            this.offerTypeRepository = offerTypeRepository;
        }

        public IList<TenancyTypeQueryResult> Handle(TenancyTypeQuery message)
        {
            IList<TenancyTypeQueryResult> offerTypes =
                this.offerTypeRepository.Get()
                    .Select(x => new TenancyTypeQueryResult
                    {
                        Id = x.Id,
                        EnumCode = x.EnumCode
                    })
                    .ToList();

            return offerTypes;
        }
    }
}