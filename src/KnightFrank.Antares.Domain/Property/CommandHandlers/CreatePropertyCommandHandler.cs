namespace KnightFrank.Antares.Domain.Property.CommandHandlers
{
    using System;

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
    public class CreatePropertyCommandHandler : IRequestHandler<CreatePropertyCommand, Guid>
    {
        private readonly IGenericRepository<Property> propertyRepository;

        private readonly IDomainValidator<CreatePropertyCommand> domainValidator;
        private readonly IAddressValidator addressValidator;
        private readonly IEnumTypeItemValidator enumTypeItemValidator;
        private readonly IEntityValidator entityValidator;

        public CreatePropertyCommandHandler(
            IGenericRepository<Property> propertyRepository,
            IDomainValidator<CreatePropertyCommand> domainValidator,
            IAddressValidator addressValidator,
            IEnumTypeItemValidator enumTypeItemValidator,
            IEntityValidator entityValidator)
        {
            this.propertyRepository = propertyRepository;
            this.domainValidator = domainValidator;
            this.addressValidator = addressValidator;
            this.enumTypeItemValidator = enumTypeItemValidator;
            this.entityValidator = entityValidator;
        }

        public Guid Handle(CreatePropertyCommand message)
        {
            this.addressValidator.Validate(message.Address);

            this.enumTypeItemValidator.ItemExists(EnumType.Division, message.DivisionId);

            this.entityValidator.EntityExists<PropertyType>(message.PropertyTypeId);

            ValidationResult validationResult = this.domainValidator.Validate(message);

            if (!validationResult.IsValid)
            {
                throw new DomainValidationException(validationResult.Errors);
            }

            var property = Mapper.Map<Property>(message);
            
            this.propertyRepository.Add(property);
            this.propertyRepository.Save();

            return property.Id;
        }
    }
}