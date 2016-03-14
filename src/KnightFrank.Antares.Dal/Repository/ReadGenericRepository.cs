namespace KnightFrank.Antares.Dal.Repository
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;

    using KnightFrank.Antares.Dal.Model;

    public class ReadGenericRepository<T> : IReadGenericRepository<T>
        where T : BaseEntity
    {
        protected KnightFrankContext DbContext;

        public ReadGenericRepository(KnightFrankContext context)
        {
            this.DbContext = context;
        }

        public IQueryable<T> Get()
        {
            return this.DbContext.Set<T>();
        }

        public IQueryable<T> GetWithInclude(params Expression<Func<T, object>>[] paths)
        {
            IQueryable<T> queryable = this.Get();
            if (paths != null)
            {
                queryable = paths.Aggregate(queryable, (current, path) => current.Include(path));
            }

            return queryable;
        }
    }
}
