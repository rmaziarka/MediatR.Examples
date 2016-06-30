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
            switch (entityType)
            {
                case EntityTypeEnum.Property:
                    return this.kernel.Get<PropertyLatestViewDataProvider>();
                case EntityTypeEnum.Activity:
                    return this.kernel.Get<ActivityLatestViewDataProvider>();
                case EntityTypeEnum.Requirement:
                    return this.kernel.Get<RequirementLatestViewDataProvider>();
                case EntityTypeEnum.Company:
                    return this.kernel.Get<CompanyLatestViewDataProvider>();
            }

            throw new NotImplementedException();
        }

        public T GetInstance<T>()
        {
            return this.kernel.Get<T>();
        }
    }
}
