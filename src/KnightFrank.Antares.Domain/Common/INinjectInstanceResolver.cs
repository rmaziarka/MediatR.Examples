namespace KnightFrank.Antares.Domain.Common
{
    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Repository;

    public interface INinjectInstanceResolver
    {
        IGenericRepository<T> GetEntityGenericRepository<T>() where T : BaseEntity;
    }
}