namespace KnightFrank.Antares.Domain.AttributeConfiguration.Common
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentValidation;
    using FluentValidation.Results;

    using KnightFrank.Antares.Domain.AttributeConfiguration.EntityConfigurations;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Fields;

    //TODO: do it more generic
    public class AttributeValidator
    {
        public void Validate(object entity)
        {
            IControlsConfiguration<PropertyType, ActivityType> configuration = new ActivityControlsConfiguration();
            IList<InnerFieldState> innerFieldStates = configuration.GetInnerFieldsState(PageType.Create, PropertyType.Flat, ActivityType.Lettings, entity);
            foreach (InnerFieldState innerFieldState in innerFieldStates)
            {
                foreach (IValidator validator in innerFieldState.Validators)
                {
                    ValidationResult validationResult = validator.Validate(entity);

                    if (!validationResult.IsValid)
                    {
                        throw new ValidationException(validationResult.Errors.ToList());
                    }
                }
            }
        }
    }
}
