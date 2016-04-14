namespace KnightFrank.Antares.Domain.Requirement.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Dal.Repository;
    using Commands;

    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Domain.Common;
    using KnightFrank.Antares.Domain.Common.Exceptions;

    using MediatR;

    public class CreateRequirementCommandHandler : IRequestHandler<CreateRequirementCommand, Guid>
    {
        private readonly IGenericRepository<Requirement> requirementRepository;

        private readonly IGenericRepository<Contact> contactRepository;

        private readonly IDomainValidator<CreateRequirementCommand> createRequirementCommandDomainValidator;

        public CreateRequirementCommandHandler(IGenericRepository<Requirement> requirementRepository, IGenericRepository<Contact> contactRepository, IDomainValidator<CreateRequirementCommand>  createRequirementCommandDomainValidator)
        {
            this.requirementRepository = requirementRepository;
            this.contactRepository = contactRepository;
            this.createRequirementCommandDomainValidator = createRequirementCommandDomainValidator;
        }

        public Guid Handle(CreateRequirementCommand message)
        {
            ValidationResult validationResult = this.createRequirementCommandDomainValidator.Validate(message);
            if (!validationResult.IsValid)
            {
                throw new DomainValidationException(validationResult.Errors);
            }

            var requirement = AutoMapper.Mapper.Map<Requirement>(message);

            List<Contact> existingContacts = this.contactRepository.FindBy(x => message.ContactIds.Any(id => id == x.Id)).ToList();
            requirement.Contacts = existingContacts;
            requirement.CreateDate = DateTime.UtcNow;

            this.requirementRepository.Add(requirement);
            this.requirementRepository.Save();

            return requirement.Id;
        }
    }
}
