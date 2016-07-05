namespace KnightFrank.Antares.Domain.Contact.QueryHandlers
{
    using System.Data.Entity;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Contact.Queries;

    using MediatR;

    public class ContactQueryHandler : IRequestHandler<ContactQuery, Contact>
    {
        private readonly IReadGenericRepository<Contact> contactRepository;

        public ContactQueryHandler(IReadGenericRepository<Contact> contactRepository)
        {
            this.contactRepository = contactRepository;
        }

        public Contact Handle(ContactQuery message)
        {
            Contact contact =
                this.contactRepository.Get()
                    .Include(c => c.ContactUsers)
                    .Include(c => c.ContactUsers.Select(an => an.User))
                    .Include(c => c.ContactUsers.Select(an => an.UserType))
                    .SingleOrDefault(req => req.Id == message.Id);

            return contact;
        }
    }
}
