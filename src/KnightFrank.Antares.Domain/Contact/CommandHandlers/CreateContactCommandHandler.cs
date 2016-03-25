namespace KnightFrank.Antares.Domain.Contact.CommandHandlers
{
    using System;

    using AutoMapper;

    using KnightFrank.Antares.Dal.Model.Contact;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Contact.Commands;

    using MediatR;

    public class CreateContactCommandHandler : IRequestHandler<CreateContactCommand, Guid>
    {
        private readonly IGenericRepository<Contact> contactRepository;

        public CreateContactCommandHandler(IGenericRepository<Contact> contactRepository)
        {
            this.contactRepository = contactRepository;
        }

        public Guid Handle(CreateContactCommand message)
        {
            var contact = Mapper.Map<Contact>(message);

            this.contactRepository.Add(contact);
            this.contactRepository.Save();

            return contact.Id;
        }
    }
}
