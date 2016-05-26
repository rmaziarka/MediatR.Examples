namespace KnightFrank.Antares.Domain.Common.BusinessValidators
{
    using System;

    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Repository;

    public class ActivityTypeDefinitionValidator : IActivityTypeDefinitionValidator
    {
        private readonly IGenericRepository<ActivityTypeDefinition> activityTypeDefinitionRepository;

        public ActivityTypeDefinitionValidator(IGenericRepository<ActivityTypeDefinition> activityTypeDefinitionRepository)
        {
            this.activityTypeDefinitionRepository = activityTypeDefinitionRepository;
        }


        public void Validate(Guid activityTypeId, Guid propertyAddressCountryId, Guid propertyTypeId)
        {
            bool activityTypeDefinitionExists = this.ActivityTypeDefinitionExists(activityTypeId, propertyAddressCountryId, propertyTypeId);

            if (activityTypeDefinitionExists == false)
            {
                BusinessValidationMessage errorMessage = BusinessValidationMessage.CreateEntityNotExistMessage(typeof(ActivityTypeDefinition).Name, activityTypeId);
                throw new BusinessValidationException(errorMessage);
            }

        }

        private bool ActivityTypeDefinitionExists(Guid activityTypeId, Guid propertyAddressCountryId, Guid propertyTypeId)
        {

            bool activityTypeDefinitionExists = this.activityTypeDefinitionRepository.Any(
                x =>
                    x.CountryId == propertyAddressCountryId &&
                    x.PropertyTypeId == propertyTypeId &&
                    x.ActivityTypeId == activityTypeId);

            return activityTypeDefinitionExists;
        }
    }
}
