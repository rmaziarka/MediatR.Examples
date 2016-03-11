namespace KnightFrank.Antares.Dal.Repository
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using KnightFrank.Antares.Dal.Model;

    public interface IReadGenericRepository<T>
        where T : BaseEntity
    {
        IQueryable<T> Get();

        IQueryable<T> GetWithInclude(params Expression<Func<T, object>>[] paths);
    }
}
