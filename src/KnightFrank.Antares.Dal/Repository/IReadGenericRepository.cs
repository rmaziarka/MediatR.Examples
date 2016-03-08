namespace KnightFrank.Antares.Dal.Repository
{
    using System.Linq;

    using KnightFrank.Antares.Dal.Model;

    public interface IReadGenericRepository<out T> where T : BaseEntity
    {
        IQueryable<T> Query();
    }
}