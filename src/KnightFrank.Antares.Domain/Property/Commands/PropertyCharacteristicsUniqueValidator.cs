namespace KnightFrank.Antares.Domain.Property.Commands
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentValidation;
    using FluentValidation.Results;

    public class PropertyCharacteristicsUniqueValidator : AbstractValidator<IList<CreateOrUpdatePropertyCharacteristic>>
    {
        public PropertyCharacteristicsUniqueValidator()
        {
            this.Custom(this.PropertyCharacteristicsAreUnique);
        }

        private ValidationFailure PropertyCharacteristicsAreUnique(IList<CreateOrUpdatePropertyCharacteristic> propertyCharacteristics)
        {
            int uniqueCharacteristicId = propertyCharacteristics.Select(pc => pc.CharacteristicId).Distinct().ToList().Count;

            return uniqueCharacteristicId == propertyCharacteristics.Count
                       ? null
                       : new ValidationFailure(nameof(propertyCharacteristics), "Property characteristic are duplicated.") {ErrorCode = "propertyCharacteristics_duplicated" };
        }
    }
}