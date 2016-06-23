namespace KnightFrank.Antares.Domain.Company.QueryHandlers
{
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Company;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Company.Queries;

    using MediatR;

    public class CompanyContactsQueryHandler : IRequestHandler<CompanyContactsQuery, IEnumerable<CompanyContact>>
    {
        private readonly IReadGenericRepository<CompanyContact> companyContactRepository;

        public CompanyContactsQueryHandler(IReadGenericRepository<CompanyContact> companyContactRepository)
        {
            this.companyContactRepository = companyContactRepository;
        }

        public IEnumerable<CompanyContact> Handle(CompanyContactsQuery message)
        {
            return this.companyContactRepository.GetWithInclude(x => x.Company, x => x.Contact);
        }
    }
}