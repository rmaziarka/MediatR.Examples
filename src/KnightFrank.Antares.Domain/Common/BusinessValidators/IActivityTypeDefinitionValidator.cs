
namespace KnightFrank.Antares.Domain.Common.BusinessValidators
{
    using System;
    public interface IActivityTypeDefinitionValidator
    {
        void Validate(Guid activityTypeId, Guid propertyAddressCountryId, Guid propertyTypeId);
    }
}