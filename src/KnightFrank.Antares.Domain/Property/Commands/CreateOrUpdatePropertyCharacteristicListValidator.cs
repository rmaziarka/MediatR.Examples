namespace KnightFrank.Antares.Domain.Property.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Property.Characteristics;
    using KnightFrank.Antares.Dal.Repository;

    public class CreateOrUpdatePropertyCharacteristicListValidator
    {
        private readonly IGenericRepository<CharacteristicGroupUsage> characteristicGroupUsageRepository;

        public CreateOrUpdatePropertyCharacteristicListValidator(
            IGenericRepository<CharacteristicGroupUsage> characteristicGroupUsageRepository)
        {
            this.characteristicGroupUsageRepository = characteristicGroupUsageRepository;
        }

        public ValidationFailure PropertyCharacteristicsAreUnique(
            IList<CreateOrUpdatePropertyCharacteristic> propertyCharacteristics)
        {
            int uniqueCharacteristicId = propertyCharacteristics.Select(pc => pc.CharacteristicId).Distinct().ToList().Count;

            return uniqueCharacteristicId == propertyCharacteristics.Count
                       ? null
                       : new ValidationFailure(nameof(propertyCharacteristics), "Property characteristic are duplicated.");
        }

        public ValidationFailure CharacteristicAreInCharacteristicGroupUsage(
            Guid propertyTypeId,
            Guid countryId,
            IList<CreateOrUpdatePropertyCharacteristic> propertyCharacteristics)
        {
            List<Guid> commandCharacteristicIds = propertyCharacteristics.Select(pc => pc.CharacteristicId).ToList();

            List<Guid> characteristicIds =
                this.characteristicGroupUsageRepository.GetWithInclude(
                    cgu => cgu.PropertyTypeId == propertyTypeId && cgu.CountryId == countryId,
                    cgu => cgu.CharacteristicGroupItems)
                    .SelectMany(x => x.CharacteristicGroupItems)
                    .Select(x => x.CharacteristicId)
                    .Distinct()
                    .ToList();

            return commandCharacteristicIds.Except(characteristicIds).Any()
                       ? new ValidationFailure(
                             nameof(propertyCharacteristics),
                             "Property characteristics are invalid for property configuration.")
                       : null;
        }
    }
}
