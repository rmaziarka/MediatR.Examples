namespace KnightFrank.Antares.Domain.Common.Validator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentValidation;
    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Repository;

    public class ContactValidator : AbstractValidator<ICollection<Guid>>
    {
        private readonly IGenericRepository<Contact> contactRepository;

        public ContactValidator(IGenericRepository<Contact> contactRepository)
        {
            this.contactRepository = contactRepository;

            this.Custom(this.ContactsExistValidator);
        }

        private ValidationFailure ContactsExistValidator(ICollection<Guid> contactIds)
        {
            IEnumerable<Contact> contacts = this.contactRepository.FindBy(x => contactIds.Any(id => id == x.Id));
            return !contacts.Count().Equals(contactIds.Count)
                ? new ValidationFailure(nameof(contactIds), "Contact list is invalid.")
                {
                    ErrorCode = "contactsinvalid_error"
                }
                : null;
        }
    }
}