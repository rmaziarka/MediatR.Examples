namespace KnightFrank.Antares.Domain.Property.CommandHandlers
{
    using System;
    using System.Linq;

    using AutoMapper;

    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common;
    using KnightFrank.Antares.Domain.Common.Exceptions;
    using KnightFrank.Antares.Domain.Property.Commands;

    using MediatR;
    public class CreatePropertyCommandHandler : IRequestHandler<CreatePropertyCommand, Guid>
    {
        private readonly IGenericRepository<Property> propertyRepository;
        private readonly IGenericRepository<EnumTypeItem> enumTypeItemRepository;
        private readonly IDomainValidator<Property> propertyValidator;

        public CreatePropertyCommandHandler(IGenericRepository<Property> propertyRepository, IGenericRepository<EnumTypeItem> enumTypeItemRepository, IDomainValidator<Property> propertyValidator)
        {
            this.propertyRepository = propertyRepository;
            this.enumTypeItemRepository = enumTypeItemRepository;
            this.propertyValidator = propertyValidator;
        }

        public Guid Handle(CreatePropertyCommand message)
        {
            var property = Mapper.Map<Property>(message);
            
            var division = this.enumTypeItemRepository.FindBy(x => x.Code == message.Division.Code && x.EnumType.Code == "Division").Single();
            property.Division = division;
            property.DivisionId = division.Id;

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