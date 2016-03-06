namespace KnightFrank.Antares.Dal.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using KnightFrank.Antares.Dal.Model;

    public interface IReadGenericRepository<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
    }
}