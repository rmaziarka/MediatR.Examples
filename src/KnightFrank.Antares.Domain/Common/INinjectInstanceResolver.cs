namespace KnightFrank.Antares.Domain.Common
{
    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Model.Common;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.LatestView.DataProviders;

    public interface INinjectInstanceResolver
    {
        IGenericRepository<T> GetEntityGenericRepository<T>() where T : BaseEntity;

        ILatestViewDataProvider GetLatestViewDataProvider(EntityTypeEnum entityType);

        T GetInstance<T>();
    }
}