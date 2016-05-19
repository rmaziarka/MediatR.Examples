namespace KnightFrank.Antares.Domain.Common
{
    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Repository;

    using Ninject;

    public class NinjectInstanceResolver : INinjectInstanceResolver
    {
        private readonly IKernel kernel;

        public NinjectInstanceResolver(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public IGenericRepository<T> GetEntityGenericRepository<T>() where T : BaseEntity
        {
            return this.kernel.Get<IGenericRepository<T>>();
        }
    }
}
