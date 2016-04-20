namespace KnightFrank.Antares.Dal.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Model.Common;

    public class ResourceLocalisedRepositoryProvider : IResourceLocalisedRepositoryProvider
    {
        private readonly KnightFrankContext knightFrankContext;

        public ResourceLocalisedRepositoryProvider(KnightFrankContext knightFrankContext)
        {
            this.knightFrankContext = knightFrankContext;
        }

        public IEnumerable<ICovariantReadGenericRepository<ILocalised>> GetRepositories()
        {
            Assembly assembly = typeof(BaseLocalisedEntity).Assembly;

            IList<Type> types =
                assembly.GetTypes().Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(BaseLocalisedEntity))).ToList();

            return
                types.Select(type => typeof(ReadGenericRepository<>).MakeGenericType(type))
                     .Select(
                         readGenericRepositoryType =>
                         Activator.CreateInstance(readGenericRepositoryType, this.knightFrankContext) as
                         ICovariantReadGenericRepository<ILocalised>);
        }
    }
}
