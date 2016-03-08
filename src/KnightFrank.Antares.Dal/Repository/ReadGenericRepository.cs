namespace KnightFrank.Antares.Dal.Repository
{
    using System.Linq;

    using KnightFrank.Antares.Dal.Model;

    public class ReadGenericRepository<T> : IReadGenericRepository<T> where T : BaseEntity
    {
        protected KnightFrankContext DbContext;

        public ReadGenericRepository(KnightFrankContext context)
        {
            this.DbContext = context;
        }

        public IQueryable<T> Query() 
        {
            return this.DbContext.Set<T>().AsNoTracking().AsQueryable();
        }
    }
}