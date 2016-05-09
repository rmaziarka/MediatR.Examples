namespace KnightFrank.Antares.Domain.Common.BusinessValidators
{
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

        private BusinessValidationException GetNotContainsException(ErrorMessage errorCode)
        {
            var businessMessage = new BusinessValidationMessage(errorCode);
            return new BusinessValidationException(businessMessage);
        }
    }
}
