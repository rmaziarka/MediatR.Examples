namespace KnightFrank.Antares.Domain.Company.QueryHandlers
{
    using System.Data.Entity;
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
            Company company =
                this.companyRepository
                    .Get()
                    .Include(c=>c.Contacts)
                    .Include(c=>c.ClientCareStatus)
                    .SingleOrDefault(req => req.Id == message.Id);

            return company;
        }
    }
}
