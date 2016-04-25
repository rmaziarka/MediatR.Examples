namespace KnightFrank.Antares.Domain.Property.Commands
{
    using FluentValidation;
    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Characteristics;
    using KnightFrank.Antares.Dal.Model.Resource;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common;
    using KnightFrank.Antares.Domain.Common.Validator;

    public class UpdatePropertyCommandDomainValidator : AbstractValidator<UpdatePropertyCommand>,
                                                        IDomainValidator<UpdatePropertyCommand>
    {
        private readonly CreateOrUpdatePropertyCharacteristicListValidator propertyCharacteristicListValidator;

        public UpdatePropertyCommandDomainValidator(
            IGenericRepository<Country> countryRepository,
            IGenericRepository<PropertyType> propertyTypeRepository,
            IGenericRepository<CharacteristicGroupUsage> characteristicGroupUsageRepository,
            IDomainValidator<CreateOrUpdatePropertyCharacteristic> propertyCharacteristicDomainValidator)
        {
            this.propertyCharacteristicListValidator =
                new CreateOrUpdatePropertyCharacteristicListValidator(characteristicGroupUsageRepository);

            this.RuleFor(x => x.PropertyTypeId).SetValidator(new PropertyTypeValidator(propertyTypeRepository));
            this.RuleFor(x => x.Address.CountryId).SetValidator(new CountryValidator(countryRepository));

            this.When(
                x => x.PropertyCharacteristics != null,
                () =>
                    {
                        this.Custom(this.PropertyCharacteristicsAreUnique);
                        this.Custom(this.CharacteristicAreInCharacteristicGroupUsage);
                        this.RuleFor(x => x.PropertyCharacteristics).SetCollectionValidator(propertyCharacteristicDomainValidator);
                    });
        }

        private ValidationFailure PropertyCharacteristicsAreUnique(UpdatePropertyCommand command)
        {
            return this.propertyCharacteristicListValidator.PropertyCharacteristicsAreUnique(command.PropertyCharacteristics);
        }

        private ValidationFailure CharacteristicAreInCharacteristicGroupUsage(UpdatePropertyCommand command)
        {
            return this.propertyCharacteristicListValidator.CharacteristicAreInCharacteristicGroupUsage(
                command.PropertyTypeId,
                command.Address.CountryId,
                command.PropertyCharacteristics);
        }
    }
}
