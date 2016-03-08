namespace KnightFrank.Antares.Dal.Repository
{
    using KnightFrank.Antares.Dal.Model;

    public interface IGenericRepository<T> : IReadGenericRepository<T> where T : BaseEntity
    {
        T Add(T entity);
        T Delete(T entity);
        void Edit(T entity);
        void Save();
    }
}