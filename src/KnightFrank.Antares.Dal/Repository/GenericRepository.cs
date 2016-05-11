namespace KnightFrank.Antares.Dal.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Model.Common;

    [ExcludeFromCodeCoverage]
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

        public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return this.dbSet.Where(predicate).AsEnumerable();
        }

        public T GetById(Guid id)
        {
            return this.dbSet.SingleOrDefault(entity => entity.Id.Equals(id));
        }

        public IEnumerable<T> GetWithInclude(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] paths)
        {
            IQueryable<T> queryable = this.dbSet.Where(predicate);
            if (paths != null)
            {
                queryable = paths.Aggregate(queryable, (current, path) => current.Include(path));
            }

            return queryable.AsEnumerable();
        }

        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return this.dbSet.Any(predicate);
        }

        public virtual void Save()
        {
            DateTime utcNow = DateTime.UtcNow;

            this.dbContext.ChangeTracker
                        .Entries<IAuditableEntity>()
                        .Where(e => e.State == EntityState.Added)
                        .Select(e => e.Entity)
                        .ToList()
                        .ForEach(e =>
                        {
                            e.CreatedDate = utcNow;
                            e.LastModifiedDate = utcNow;
                        });

            this.dbContext.ChangeTracker
                        .Entries<IAuditableEntity>()
                        .Where(e => e.State == EntityState.Modified)
                        .Select(e => e.Entity)
                        .ToList()
                        .ForEach(e =>
                        {
                            e.LastModifiedDate = utcNow;
                        });

            this.dbContext.SaveChanges();
        }
    }
}