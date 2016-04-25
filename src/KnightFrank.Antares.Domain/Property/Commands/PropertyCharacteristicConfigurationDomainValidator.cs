namespace KnightFrank.Antares.Domain.Property.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentValidation;
    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Property.Characteristics;
    using KnightFrank.Antares.Dal.Repository;

    public class PropertyCharacteristicConfigurationDomainValidator :
        AbstractValidator<IList<CreateOrUpdatePropertyCharacteristic>>
    {
        private readonly IGenericRepository<CharacteristicGroupUsage> characteristicGroupUsageRepository;

        private readonly Guid countryId;

        private readonly Guid propertyTypeId;

        public PropertyCharacteristicConfigurationDomainValidator(
            IGenericRepository<CharacteristicGroupUsage> characteristicGroupUsageRepository,
            Guid propertyTypeId,
            Guid countryId)
        {
            this.characteristicGroupUsageRepository = characteristicGroupUsageRepository;
            this.propertyTypeId = propertyTypeId;
            this.countryId = countryId;

            this.Custom(this.CharacteristicAreInCharacteristicGroupUsage);
        }

        private ValidationFailure CharacteristicAreInCharacteristicGroupUsage(
            IList<CreateOrUpdatePropertyCharacteristic> propertyCharacteristics)
        {
            List<Guid> commandCharacteristicIds = propertyCharacteristics.Select(pc => pc.CharacteristicId).ToList();

            List<Guid> characteristicIds =
                this.characteristicGroupUsageRepository.GetWithInclude(
                    cgu => cgu.PropertyTypeId == this.propertyTypeId && cgu.CountryId == this.countryId,
                    cgu => cgu.CharacteristicGroupItems)
                    .SelectMany(x => x.CharacteristicGroupItems)
                    .Select(x => x.CharacteristicId)
                    .Distinct()
                    .ToList();

            return commandCharacteristicIds.Except(characteristicIds).Any()
                       ? new ValidationFailure(
                             nameof(propertyCharacteristics),
                             "Property characteristics are invalid for property configuration.")
                             {
                                 ErrorCode =
                                     "propertyCharacteristics_notconfigured"
                             }
                       : null;
        }
    }
}
