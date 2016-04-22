namespace KnightFrank.Antares.Domain.Property.CommandHandlers
{
    using System;
    using System.Linq;

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

        private readonly IDomainValidator<CreatePropertyCommand> domainValidator;

        public CreatePropertyCommandHandler(IGenericRepository<Property> propertyRepository, IDomainValidator<CreatePropertyCommand> domainValidator)
        {
            this.propertyRepository = propertyRepository;
            this.domainValidator = domainValidator;
        }

        public Guid Handle(CreatePropertyCommand message)
        {
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