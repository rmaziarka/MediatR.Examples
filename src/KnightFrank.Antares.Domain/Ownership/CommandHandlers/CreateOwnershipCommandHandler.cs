namespace KnightFrank.Antares.Domain.Ownership.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common;
    using KnightFrank.Antares.Domain.Common.Exceptions;
    using KnightFrank.Antares.Domain.Ownership.Commands;

    using MediatR;

    public class CreateOwnershipCommandHandler : IRequestHandler<CreateOwnershipCommand, Guid>
    {
        private readonly IGenericRepository<Ownership> ownershipRepository;
        private readonly IGenericRepository<Contact> contactRepository;
        private readonly IDomainValidator<CreateOwnershipCommand> ownershipDomainValidator;

        public CreateOwnershipCommandHandler(IGenericRepository<Ownership> ownershipRepository,
            IGenericRepository<Contact> contactRepository,
            IDomainValidator<CreateOwnershipCommand> ownershipDomainValidator)
        {
            this.ownershipRepository = ownershipRepository;
            this.contactRepository = contactRepository;
            this.ownershipDomainValidator = ownershipDomainValidator;
        }

        public Guid Handle(CreateOwnershipCommand message)
        {
            ValidationResult validationResult = this.ownershipDomainValidator.Validate(message);
            if (!validationResult.IsValid)
            {
                throw new DomainValidationException(validationResult.Errors);
            }

            var ownership = Mapper.Map<Ownership>(message);

            List<Contact> existingContacts = this.contactRepository.FindBy(x => message.ContactIds.Any(id => id == x.Id)).ToList();
            ownership.Contacts = existingContacts;

            this.ownershipRepository.Add(ownership);
            this.ownershipRepository.Save();

            return ownership.Id;
        }
    }
}
