namespace KnightFrank.Antares.Dal.Repository
{
    using System.Data.Entity;

    using KnightFrank.Antares.Dal.Model;

    public class GenericRepository<T> : ReadGenericRepository<T>, IGenericRepository<T> where T : BaseEntity
    {
        public GenericRepository(KnightFrankContext context) : base(context) { }

        public virtual T Add(T entity)
        {
            return this.DbSet.Add(entity);
        }

        public virtual T Delete(T entity)
        {
            return this.DbSet.Remove(entity);
        }

        public virtual void Edit(T entity)
        {
            this.DbContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Save()
        {
            this.DbContext.SaveChanges();
        }
    }
}