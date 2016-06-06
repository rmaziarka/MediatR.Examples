namespace KnightFrank.Antares.Domain.LatestView.DataProviders
{
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.LatestView;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.LatestView.QueryResults;

    public class RequirementLatestViewDataProvider : BaseLatestViewDataProvider<Requirement>
    {
        private readonly IReadGenericRepository<Requirement> requirementRepository;

        public RequirementLatestViewDataProvider(IReadGenericRepository<Requirement> requirementRepository)
        {
            this.requirementRepository = requirementRepository;
        }

        public override IQueryable<Requirement> GetEntitiesWithIncludes()
        {
            return this.requirementRepository.GetWithInclude(x => x.Contacts);
        }

        public override LatestViewData CreateLatestViewData(Requirement requirement, LatestView latestView)
        {
            return new LatestViewData
            {
                Id = latestView.EntityId,
                CreatedDate = latestView.CreatedDate,
                Data = requirement.Contacts
            };
        }
    }
}