namespace KnightFrank.Antares.Domain.AttributeConfiguration.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentValidation;
    using FluentValidation.Results;

    using KnightFrank.Antares.Domain.AttributeConfiguration.EntityConfigurations;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Fields;

    public class AttributeValidator<TKey1, TKey2> : IAttributeValidator<TKey1, TKey2>
    {
        private readonly IControlsConfiguration<TKey1, TKey2> configuration;
        public AttributeValidator(IControlsConfiguration<TKey1, TKey2> configuration)
        {
            this.configuration = configuration;
        }

        public void Validate(PageType pageType, TKey1 key1, TKey2 key2, object entity)
        {
            IList<InnerFieldState> innerFieldStates = this.configuration.GetInnerFieldsState(pageType, key1, key2, entity);

            if (!innerFieldStates.Any())
            {
                throw new NotSupportedException(
                    $"There is no attribute {pageType} configuration for the followign pair: {key1} {key2}");
            }

            foreach (InnerFieldState innerFieldState in innerFieldStates.Where(x => !x.Hidden || !x.Readonly))
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
