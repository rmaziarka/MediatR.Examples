namespace KnightFrank.Antares.Domain.Company.QueryHandlers
{
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Company;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Company.Queries;

    using MediatR;

    public class CompanyQueryHandler : IRequestHandler<CompanyQuery, Company>
    {
        private readonly IReadGenericRepository<Company> companyRepository;

        public CompanyQueryHandler(IReadGenericRepository<Company> companyRepository)
        {
            this.companyRepository = companyRepository;
        }

        public Company Handle(CompanyQuery message)
        {
            Company company = this.companyRepository
                .GetWithInclude(x => x.CompaniesContacts.Select(y => y.Contact), x => x.ClientCareStatus, x => x.CompanyType, x => x.CompanyCategory, x => x.RelationshipManger)
                .SingleOrDefault(com => com.Id == message.Id);

            return company;
        }
    }
}
