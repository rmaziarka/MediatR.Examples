namespace KnightFrank.Antares.Domain.Common.BusinessValidators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CollectionValidator : ICollectionValidator
    {
        public void CollectionContainsAll<T>(IEnumerable<T> collection, IEnumerable<T> set, ErrorMessage errorCode)
        {
            if (set.Any(x => collection.All(c => !c.Equals(x))))
            {
                throw this.GetNotContainsException(errorCode);
            }
        }

        public void RangeDoesNotOverlap(List<Range<DateTime>> existingDates, Range<DateTime> range, ErrorMessage errorCode)
        {
            bool overlap = existingDates.Any(existingDate => existingDate.IsOverlapped(range));
            if (overlap)
            {
                throw new BusinessValidationException(errorCode);
            }
        }

        public void CollectionIsUnique<T>(ICollection<T> collection, ErrorMessage errorCode)
        {
            int uniqueCount = collection.Distinct().ToList().Count;
            if (uniqueCount != collection.Count)
            {
                throw new BusinessValidationException(errorCode);
            }
        }

        private BusinessValidationException GetNotContainsException(ErrorMessage errorCode)
        {
            var businessMessage = new BusinessValidationMessage(errorCode);
            return new BusinessValidationException(businessMessage);
        }

    }
}
