namespace KnightFrank.Antares.Domain.Common.BusinessValidators
{
    using System;
    using System.Collections.Generic;

    public interface ICollectionValidator
    {
        void CollectionContainsAll<T>(IEnumerable<T> collection, IEnumerable<T> set, ErrorMessage errorCode);

        void RangeDoesNotOverlap(List<Range<DateTime>> existingDates, Range<DateTime> range, ErrorMessage errorCode);
    }
}