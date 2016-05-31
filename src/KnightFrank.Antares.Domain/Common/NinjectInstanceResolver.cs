namespace KnightFrank.Antares.Domain.Common
{
    using System;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Model.Common;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.LatestView.DataProviders;

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

        public ILatestViewDataProvider GetLatestViewDataProvider(EntityTypeEnum entityType)
        {
            if (entityType == EntityTypeEnum.Property)
            {
                return this.kernel.Get<PropertyLatestViewDataProvider>();
            }

            throw new NotImplementedException();
        }
    }
}
