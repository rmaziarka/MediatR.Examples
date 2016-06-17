namespace KnightFrank.Antares.Domain.AttributeConfiguration.Common
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using FluentValidation;
    using FluentValidation.Results;

    using KnightFrank.Antares.Domain.AttributeConfiguration.EntityConfigurations;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Fields;

    public class AttributeValidator<TKey> : IAttributeValidator<TKey> where TKey : IStructuralEquatable, IStructuralComparable, IComparable
    {
        private readonly IControlsConfiguration<TKey> configuration;
        public AttributeValidator(IControlsConfiguration<TKey> configuration)
        {
            this.configuration = configuration;
        }

        public void Validate(PageType pageType, TKey key, object entity)
        {
            IList<InnerFieldState> innerFieldStates = this.configuration.GetInnerFieldsState(pageType, key, entity);

            if (!innerFieldStates.Any())
            {
                throw new NotSupportedException(
                    $"There is no attribute {pageType} configuration for the followign pair: {key}");
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
