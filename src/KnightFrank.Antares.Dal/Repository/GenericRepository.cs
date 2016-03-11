namespace KnightFrank.Antares.Dal.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model;

    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly KnightFrankContext dbContext;
        private readonly DbSet<T> dbSet;

        public GenericRepository(KnightFrankContext dbContext)
        {
            this.dbContext = dbContext;
            this.dbSet = dbContext.Set<T>();
        }

        public virtual T Add(T entity)
        {
            return this.dbSet.Add(entity);
        }

        public virtual T Delete(T entity)
        {
            return this.dbSet.Remove(entity);
        }

        public virtual void Edit(T entity)
        {
            this.dbContext.Entry(entity).State = EntityState.Modified;
        }

        public IEnumerable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return this.dbSet.Where(predicate).AsEnumerable();
        }

        public T GetById(Guid id)
        {
            return this.FindBy(entity => entity.Id.Equals(id)).First();
        }

        public virtual void Save()
        {
            this.dbContext.SaveChanges();
        }
    }
}