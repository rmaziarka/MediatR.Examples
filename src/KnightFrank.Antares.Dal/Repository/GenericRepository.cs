namespace KnightFrank.Antares.Dal.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model;

    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected KnightFrankContext Entities;
        protected readonly IDbSet<T> Dbset;

        public GenericRepository(KnightFrankContext context)
        {
            this.Entities = context;
            this.Dbset = context.Set<T>();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return this.Dbset.AsEnumerable<T>();
        }

        public IEnumerable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {

            IEnumerable<T> query = this.Dbset.Where(predicate).AsEnumerable();
            return query;
        }

        public virtual T Add(T entity)
        {
            return this.Dbset.Add(entity);
        }

        public virtual T Delete(T entity)
        {
            return this.Dbset.Remove(entity);
        }

        public virtual void Edit(T entity)
        {
            this.Entities.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Save()
        {
            this.Entities.SaveChanges();
        }
    }
}