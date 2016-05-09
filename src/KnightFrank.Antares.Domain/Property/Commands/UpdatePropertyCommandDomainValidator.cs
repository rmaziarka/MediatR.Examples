namespace KnightFrank.Antares.Domain.Property.Commands
{
    using FluentValidation;
    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Characteristics;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common;
    using KnightFrank.Antares.Domain.Common.Validator;

    public class UpdatePropertyCommandDomainValidator : AbstractValidator<UpdatePropertyCommand>,
                                                        IDomainValidator<UpdatePropertyCommand>
    {
        public UpdatePropertyCommandDomainValidator(IGenericRepository<PropertyType> propertyTypeRepository,
            IGenericRepository<CharacteristicGroupUsage> characteristicGroupUsageRepository,
            IDomainValidator<CreateOrUpdatePropertyCharacteristic> propertyCharacteristicDomainValidator)
        {
            this.RuleFor(x => x.PropertyTypeId).SetValidator(new PropertyTypeValidator(propertyTypeRepository));
            
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
