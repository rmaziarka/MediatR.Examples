namespace KnightFrank.Antares.Domain.Property.Commands
{
    using FluentValidation;

    using KnightFrank.Antares.Dal.Model.Property.Characteristics;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common;

    public class CreatePropertyCommandDomainValidator : AbstractValidator<CreatePropertyCommand>,
                                                        IDomainValidator<CreatePropertyCommand>
    {
        public CreatePropertyCommandDomainValidator(IGenericRepository<CharacteristicGroupUsage> characteristicGroupUsageRepository,
            IDomainValidator<CreateOrUpdatePropertyCharacteristic> propertyCharacteristicDomainValidator)
        {
            this.When(
                x => x.PropertyCharacteristics != null,
                () =>
                    {
                        this.RuleFor(x => x.PropertyCharacteristics)
                            .SetValidator(
                                x =>
                                new PropertyCharacteristicConfigurationDomainValidator(
                                    characteristicGroupUsageRepository,
                                    x.PropertyTypeId,
                                    x.Address.CountryId));

                        this.RuleFor(x => x.PropertyCharacteristics).SetCollectionValidator(propertyCharacteristicDomainValidator);
                    });
        }
    }
}
