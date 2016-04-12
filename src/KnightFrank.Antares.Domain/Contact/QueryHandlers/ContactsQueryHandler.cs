namespace KnightFrank.Antares.Domain.Contact.QueryHandlers
{
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Contact.Queries;

    using MediatR;

    public class ContactsQueryHandler : IRequestHandler<ContactsQuery, IEnumerable<Contact>>
    {
        private readonly IReadGenericRepository<Contact> contactRepository;

        public ContactsQueryHandler(IReadGenericRepository<Contact> contactRepository)
        {
            this.contactRepository = contactRepository;
        }

        public IEnumerable<Contact> Handle(ContactsQuery message)
        {
            return this.contactRepository.Get();
        }
    }
}