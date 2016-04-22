namespace KnightFrank.Antares.Domain.Property.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentValidation;
    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Characteristics;
    using KnightFrank.Antares.Dal.Model.Resource;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common;

    public class CreatePropertyCommandDomainValidator : AbstractValidator<CreatePropertyCommand>,
                                                        IDomainValidator<CreatePropertyCommand>
    {
        private readonly IGenericRepository<CharacteristicGroupUsage> characteristicGroupUsageRepository;

        private readonly IGenericRepository<Country> countryRepository;

        private readonly IGenericRepository<PropertyType> propertyTypeRepository;

        public CreatePropertyCommandDomainValidator(
            IGenericRepository<Country> countryRepository,
            IGenericRepository<PropertyType> propertyTypeRepository,
            IGenericRepository<CharacteristicGroupUsage> characteristicGroupUsageRepository,
            IDomainValidator<CreateOrUpdatePropertyCharacteristic> propertyCharacteristicDomainValidator)
        {
            this.countryRepository = countryRepository;
            this.propertyTypeRepository = propertyTypeRepository;
            this.characteristicGroupUsageRepository = characteristicGroupUsageRepository;

            this.Custom(this.CountryExists);
            this.Custom(this.PropertyTypeExists);

            this.When(
                x => x.PropertyCharacteristics != null,
                () =>
                    {
                        this.Custom(this.PropertyCharacteristicsAreUnique);
                        this.Custom(this.CharacteristicAreInCharacteristicGroupUsage);
                        this.RuleFor(x => x.PropertyCharacteristics).SetCollectionValidator(propertyCharacteristicDomainValidator);
                    });
        }

        private ValidationFailure CountryExists(CreatePropertyCommand command)
        {
            Country country = this.countryRepository.GetById(command.Address.CountryId);

            return country == null ? new ValidationFailure(nameof(command.Address.CountryId), "Country does not exist.") : null;
        }

        private ValidationFailure PropertyTypeExists(CreatePropertyCommand command)
        {
            PropertyType propertyType = this.propertyTypeRepository.GetById(command.PropertyTypeId);

            return propertyType == null
                       ? new ValidationFailure(nameof(command.PropertyTypeId), "Property type does not exist.")
                       : null;
        }

        private ValidationFailure PropertyCharacteristicsAreUnique(CreatePropertyCommand command)
        {
            int uniqueCharacteristicId =
                command.PropertyCharacteristics.Select(pc => pc.CharacteristicId).Distinct().ToList().Count;

            return uniqueCharacteristicId == command.PropertyCharacteristics.Count
                       ? null
                       : new ValidationFailure(nameof(command.PropertyCharacteristics), "Property characteristic are duplicated.");
        }

        private ValidationFailure CharacteristicAreInCharacteristicGroupUsage(CreatePropertyCommand command)
        {
            Guid propertyTypeId = command.PropertyTypeId;
            Guid countryId = command.Address.CountryId;
            List<Guid> commandCharacteristicIds =
                command.PropertyCharacteristics.Select(pc => pc.CharacteristicId).ToList();

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
                             nameof(command.PropertyCharacteristics),
                             "Property characteristics are invalid for property configuration.")
                       : null;
        }
    }
}
