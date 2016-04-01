namespace KnightFrank.Antares.Dal.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using KnightFrank.Antares.Dal.Model;

    public interface IGenericRepository<T> where T : BaseEntity
    {
        T Add(T entity);

        T Delete(T entity);

        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);

        T GetById(Guid id);

        void Save();

        IEnumerable<T> GetWithInclude(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] paths);

        bool Any(Expression<Func<T, bool>> predicate);
    }
}
