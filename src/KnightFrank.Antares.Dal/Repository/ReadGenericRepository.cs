namespace KnightFrank.Antares.Dal.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model;

    public class ReadGenericRepository<T> : IReadGenericRepository<T> where T : BaseEntity
    {
        protected KnightFrankContext DbContext;
        protected readonly IDbSet<T> DbSet;

        public ReadGenericRepository(KnightFrankContext context)
        {
            this.DbContext = context;
            this.DbSet = context.Set<T>();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return this.DbSet.AsEnumerable();
        }

        public IEnumerable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return this.DbSet.Where(predicate).AsEnumerable();
        }
    }
}