namespace KnightFrank.Antares.Domain.LatestView.DataProviders
{
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.LatestView;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.LatestView.QueryResults;

    public class PropertyLatestViewDataProvider : BaseLatestViewDataProvider<Property>
    {
        private readonly IReadGenericRepository<Property> propertyRepository;

        public PropertyLatestViewDataProvider(IReadGenericRepository<Property> propertyRepository)
        {
            this.propertyRepository = propertyRepository;
        }

        public override IQueryable<Property> GetEntitiesWithIncludes()
        {
            return this.propertyRepository.GetWithInclude(x => x.Address);
        }

        public override LatestViewData CreateLatestViewData(Property property, LatestView latestView)
        {
            return new LatestViewData
            {
                Id = latestView.EntityId,
                CreatedDate = latestView.CreatedDate,
                Data = property.Address
            };
        }
    }
}