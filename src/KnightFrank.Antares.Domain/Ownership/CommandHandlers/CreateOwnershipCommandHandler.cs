namespace KnightFrank.Antares.Domain.Ownership.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Ownership.Commands;

    using MediatR;

    public class CreateOwnershipCommandHandler : IRequestHandler<CreateOwnershipCommand, Guid>
    {
        private readonly IGenericRepository<Ownership> ownershipRepository;

        private readonly IGenericRepository<Contact> contactRepository;

        public CreateOwnershipCommandHandler(IGenericRepository<Ownership> ownershipRepository, IGenericRepository<Contact> contactRepository)
        {
            this.ownershipRepository = ownershipRepository;
            this.contactRepository = contactRepository;
        }

        public Guid Handle(CreateOwnershipCommand message)
        {
            var ownership = Mapper.Map<Ownership>(message);

            List<Contact> existingContacts = this.contactRepository.FindBy(x => message.ContactIds.Any(id => id == x.Id)).ToList();
            ownership.Contacts = existingContacts;

            this.ownershipRepository.Add(ownership);
            this.ownershipRepository.Save();

            return ownership.Id;
        }
    }
}
