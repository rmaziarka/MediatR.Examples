namespace KnightFrank.Antares.Dal.Repository
{
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Common;

    public interface IResourceLocalisedRepositoryProvider
    {
        IEnumerable<ICovariantReadGenericRepository<ILocalised>> GetRepositories();
    }
}
