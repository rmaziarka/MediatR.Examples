namespace KnightFrank.Antares.Domain.Property.CommandHandlers
{
    using System;

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
        private readonly IGenericRepository<Property> propertyRepository;
        private readonly IDomainValidator<Property> propertyValidator;

        public UpdatePropertyCommandHandler(
            IGenericRepository<Property> propertyRepository, IDomainValidator<Property> propertyValidator)
        {
            this.propertyRepository = propertyRepository;
            this.propertyValidator = propertyValidator;
        }

        public Guid Handle(UpdatePropertyCommand message)
        {
            Property property = this.propertyRepository.GetById(message.Id);

            if (property == null)
            {
                throw new ResourceNotFoundException("Property does not exist", message.Id);
            }

            Mapper.Map(message, property);

            ValidationResult validationResult = this.propertyValidator.Validate(property);
            if (!validationResult.IsValid)
            {
                throw new DomainValidationException(validationResult.Errors);
            }

            this.propertyRepository.Save();

            return property.Id;
        }
    }
}