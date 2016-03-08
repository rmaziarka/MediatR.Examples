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
        void Edit(T entity);
        IEnumerable<T> GetAll();
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
        void Save();
    }
}