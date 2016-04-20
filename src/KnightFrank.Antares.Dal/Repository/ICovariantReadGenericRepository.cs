namespace KnightFrank.Antares.Dal.Repository
{
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Common;

    public interface ICovariantReadGenericRepository<out T>
        where T : IBaseEntity
    {
        IQueryable<T> Get();
    }
}
