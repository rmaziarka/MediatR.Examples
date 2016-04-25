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
    using KnightFrank.Antares.Domain.Common.Exceptions;
    using KnightFrank.Antares.Domain.Property.Commands;

    using MediatR;

    public class UpdatePropertyCommandHandler : IRequestHandler<UpdatePropertyCommand, Guid>
    {
        private readonly IDomainValidator<UpdatePropertyCommand> domainValidator;

        private readonly IGenericRepository<PropertyCharacteristic> propertyCharacteristicRepository;

        private readonly IGenericRepository<Property> propertyRepository;

        public UpdatePropertyCommandHandler(
            IGenericRepository<Property> propertyRepository,
            IGenericRepository<PropertyCharacteristic> propertyCharacteristicRepository,
            IDomainValidator<UpdatePropertyCommand> domainValidator)
        {
            this.propertyRepository = propertyRepository;
            this.propertyCharacteristicRepository = propertyCharacteristicRepository;
            this.domainValidator = domainValidator;
        }

        public Guid Handle(UpdatePropertyCommand message)
        {
            Property property =
                this.propertyRepository.GetWithInclude(x => x.Id == message.Id, x => x.PropertyCharacteristics).SingleOrDefault();

            if (property == null)
            {
                throw new ResourceNotFoundException("Property does not exist", message.Id);
            }

            ValidationResult validationResult = this.domainValidator.Validate(message);

            if (!validationResult.IsValid)
            {
                throw new DomainValidationException(validationResult.Errors);
            }

            Mapper.Map(message, property);

            List<PropertyCharacteristic> existingCharacteristics = property.PropertyCharacteristics.ToList();

            existingCharacteristics.Where(c => IsRemovedFromExistingList(c, message.PropertyCharacteristics))
                                   .ToList()
                                   .ForEach(x => this.propertyCharacteristicRepository.Delete(x));

            Enumerable.ToList(
                message.PropertyCharacteristics.Where(c => IsNewlyAddedToExistingList(c, existingCharacteristics))
                       .Select(Mapper.Map<PropertyCharacteristic>)).ForEach(p => property.PropertyCharacteristics.Add(p));

            message.PropertyCharacteristics.Where(c => IsUpdated(c, existingCharacteristics))
                   .Select(
                       c =>
                       new
                           {
                               newPropertyCharacteristic = c,
                               oldPropertyCharacteristic = GetOldCharacteristic(c, existingCharacteristics)
                           })
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
