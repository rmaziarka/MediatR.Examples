namespace KnightFrank.Antares.Domain.Requirement.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentValidation;
    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common;

    public class CreateRequirementCommandDomainValidator : AbstractValidator<CreateRequirementCommand>, IDomainValidator<CreateRequirementCommand>
    {
        private readonly IGenericRepository<Contact> contactRepository;

        public CreateRequirementCommandDomainValidator(IGenericRepository<Contact> contactRepository)
        {
            this.contactRepository = contactRepository;
            this.Custom(this.ContactsValid);
        }

        private ValidationFailure ContactsValid(CreateRequirementCommand command)
        {
            List<Guid> commandContactIds = command.Contacts.Select(x => x.Id).ToList();
            List<Guid> existingContactsIds = this.contactRepository.FindBy(x => commandContactIds.Any(id => id == x.Id)).Select(x => x.Id).ToList();

            if (!commandContactIds.SequenceEqual(existingContactsIds))
            {
                return new ValidationFailure(nameof(command.Contacts), "Invalid contact list has been provided.");
            }

            return null;
        }
    }
}
