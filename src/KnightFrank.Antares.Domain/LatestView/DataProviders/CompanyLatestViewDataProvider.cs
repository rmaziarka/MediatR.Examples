namespace KnightFrank.Antares.Domain.LatestView.DataProviders
{
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Company;
    using KnightFrank.Antares.Dal.Model.LatestView;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.LatestView.QueryResults;

    public class CompanyLatestViewDataProvider : BaseLatestViewDataProvider<Company>
    {
        private readonly IReadGenericRepository<Company> companyRepository;

        public CompanyLatestViewDataProvider(IReadGenericRepository<Company> companyRepository)
        {
            this.companyRepository = companyRepository;
        }

        public override IQueryable<Company> GetEntitiesWithIncludes()
        {
            return this.companyRepository.Get();
        }

        public override LatestViewData CreateLatestViewData(Company company, LatestView latestView)
        {
            return new LatestViewData
            {
                Id = latestView.EntityId,
                CreatedDate = latestView.CreatedDate,
                Data = company
            };
        }
    }
}