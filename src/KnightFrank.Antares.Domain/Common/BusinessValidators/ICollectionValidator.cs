namespace KnightFrank.Antares.Domain.Common.BusinessValidators
{
    using System.Collections.Generic;

    public interface ICollectionValidator
    {
        void CollectionContainsAll<T>(IEnumerable<T> collection, IEnumerable<T> set, ErrorMessage errorCode);
    }
}