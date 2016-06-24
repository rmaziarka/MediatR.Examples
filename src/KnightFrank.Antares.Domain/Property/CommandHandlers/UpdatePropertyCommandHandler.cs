namespace KnightFrank.Antares.Domain.Property.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Enums;
    using KnightFrank.Antares.Domain.Common.Exceptions;
    using KnightFrank.Antares.Domain.Property.Commands;

    using MediatR;

    public class UpdatePropertyCommandHandler : IRequestHandler<UpdatePropertyCommand, Guid>
    {
        private readonly IDomainValidator<UpdatePropertyCommand> domainValidator;

        private readonly IAddressValidator addressValidator;
        private readonly IEnumTypeItemValidator enumTypeItemValidator;
        private readonly IEntityValidator entityValidator;

        private readonly IGenericRepository<PropertyCharacteristic> propertyCharacteristicRepository;

        private readonly IGenericRepository<Property> propertyRepository;

        public UpdatePropertyCommandHandler(
            IGenericRepository<Property> propertyRepository,
            IGenericRepository<PropertyCharacteristic> propertyCharacteristicRepository,
            IDomainValidator<UpdatePropertyCommand> domainValidator,
            IAddressValidator addressValidator,
            IEnumTypeItemValidator enumTypeItemValidator,
            IEntityValidator entityValidator)
        {
            this.propertyRepository = propertyRepository;
            this.propertyCharacteristicRepository = propertyCharacteristicRepository;
            this.domainValidator = domainValidator;
            this.addressValidator = addressValidator;
            this.enumTypeItemValidator = enumTypeItemValidator;
            this.entityValidator = entityValidator;
        }

        public Guid Handle(UpdatePropertyCommand message)
        {
            this.addressValidator.Validate(message.Address);

            this.enumTypeItemValidator.ItemExists(EnumType.Division, message.DivisionId);

            this.entityValidator.EntityExists<Dal.Model.Property.PropertyType>(message.PropertyTypeId);

            Property property =
                this.propertyRepository.GetWithInclude(x => x.Id == message.Id, x => x.PropertyCharacteristics).SingleOrDefault();

            this.entityValidator.EntityExists(property, message.Id);

            ValidationResult validationResult = this.domainValidator.Validate(message);

            if (!validationResult.IsValid)
            {
                throw new DomainValidationException(validationResult.Errors);
            }

            Mapper.Map(message, property);

            List<PropertyCharacteristic> existingCharacteristics = property.PropertyCharacteristics.ToList();

            existingCharacteristics
                .Where(c => IsRemovedFromExistingList(c, message.PropertyCharacteristics))
                .ToList()
                .ForEach(x => this.propertyCharacteristicRepository.Delete(x));

            message.PropertyCharacteristics
                .Where(c => IsNewlyAddedToExistingList(c, existingCharacteristics))
                .Select(Mapper.Map<PropertyCharacteristic>)
                .ToList()
                .ForEach(p => property.PropertyCharacteristics.Add(p));

            message.PropertyCharacteristics
                .Where(c => IsUpdated(c, existingCharacteristics))
                .Select(c => new { newPropertyCharacteristic = c, oldPropertyCharacteristic = GetOldCharacteristic(c, existingCharacteristics) })
                .ToList()
                .ForEach(pair => Mapper.Map(pair.newPropertyCharacteristic, pair.oldPropertyCharacteristic));

            this.propertyRepository.Save();

            return property.Id;
        }

        private static bool IsRemovedFromExistingList(
            PropertyCharacteristic propertyCharacteristic,
            IEnumerable<CreateOrUpdatePropertyCharacteristic> characteristics)
        {
            return !characteristics.Select(c => c.CharacteristicId).Contains(propertyCharacteristic.CharacteristicId);
        }

        private static bool IsNewlyAddedToExistingList(
            CreateOrUpdatePropertyCharacteristic propertyCharacteristic,
            IEnumerable<PropertyCharacteristic> characteristics)
        {
            return !characteristics.Select(c => c.CharacteristicId).Contains(propertyCharacteristic.CharacteristicId);
        }

        private static bool IsUpdated(
            CreateOrUpdatePropertyCharacteristic propertyCharacteristic,
            IEnumerable<PropertyCharacteristic> characteristics)
        {
            return !IsNewlyAddedToExistingList(propertyCharacteristic, characteristics);
        }

        private static PropertyCharacteristic GetOldCharacteristic(
            CreateOrUpdatePropertyCharacteristic propertyCharacteristic,
            IEnumerable<PropertyCharacteristic> characteristics)
        {
            return characteristics.SingleOrDefault(c => c.CharacteristicId == propertyCharacteristic.CharacteristicId);
        }
    }
}
