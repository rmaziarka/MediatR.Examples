namespace KnightFrank.Antares.Domain.LatestView.DataProviders
{
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.LatestView;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.LatestView.QueryResults;

    public class ContactLatestViewDataProvider : BaseLatestViewDataProvider<Contact>
    {
        private readonly IReadGenericRepository<Contact> contactRepository;

        public ContactLatestViewDataProvider(IReadGenericRepository<Contact> contactRepository)
        {
            this.contactRepository = contactRepository;
        }

        public override IQueryable<Contact> GetEntitiesWithIncludes()
        {
            return this.contactRepository.Get();
        }

        public override LatestViewData CreateLatestViewData(Contact contact, LatestView latestView)
        {
            return new LatestViewData
            {
                Id = latestView.EntityId,
                CreatedDate = latestView.CreatedDate,
                Data = contact
            };
        }
    }
}