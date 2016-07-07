namespace KnightFrank.Antares.Dal.Repository
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using KnightFrank.Antares.Dal.Model.Common;

    public interface IReadGenericRepository<T> : ICovariantReadGenericRepository<T>
        where T : IBaseEntity
    {
        IQueryable<T> GetWithInclude(params Expression<Func<T, object>>[] paths);
    }
}
