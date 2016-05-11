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
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Exceptions;

    using MediatR;

    public class CreateRequirementCommandHandler : IRequestHandler<CreateRequirementCommand, Guid>
    {
        private readonly IGenericRepository<Requirement> requirementRepository;

        private readonly IGenericRepository<Contact> contactRepository;

        private readonly IAddressValidator addressValidator;

        public CreateRequirementCommandHandler(IGenericRepository<Requirement> requirementRepository, IGenericRepository<Contact> contactRepository, IAddressValidator addressValidator)
        {
            this.requirementRepository = requirementRepository;
            this.contactRepository = contactRepository;
            this.addressValidator = addressValidator;
        }

        public Guid Handle(CreateRequirementCommand message)
        {
            this.addressValidator.Validate(message.Address);

            var requirement = AutoMapper.Mapper.Map<Requirement>(message);

            List<Contact> existingContacts = this.contactRepository.FindBy(x => message.ContactIds.Any(id => id == x.Id)).ToList();
            if (!message.ContactIds.All(x => existingContacts.Select(c => c.Id).Contains(x)))
            {
                throw new BusinessValidationException(ErrorMessage.Missing_Requirement_Applicants_Id);
            }

            requirement.Contacts = existingContacts;
            requirement.CreateDate = DateTime.UtcNow;

            this.requirementRepository.Add(requirement);
            this.requirementRepository.Save();

            return requirement.Id;
        }
    }
}
