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
    public class CreatePropertyCommandHandler : IRequestHandler<CreatePropertyCommand, Guid>
    {
        private readonly IGenericRepository<Property> propertyRepository;
        private readonly IDomainValidator<Property> propertyValidator;

        public CreatePropertyCommandHandler(IGenericRepository<Property> propertyRepository, IDomainValidator<Property> propertyValidator)
        {
            this.propertyRepository = propertyRepository;
            this.propertyValidator = propertyValidator;
        }

        public Guid Handle(CreatePropertyCommand message)
        {
            var property = Mapper.Map<Property>(message);

            ValidationResult validationResult = this.propertyValidator.Validate(property);
            if (!validationResult.IsValid)
            {
                throw new DomainValidationException(validationResult.Errors);
            }

            this.propertyRepository.Add(property);
            this.propertyRepository.Save();

            return property.Id;
        }
    }
}