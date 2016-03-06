namespace KnightFrank.Antares.Domain.Contact.CommandHandlers
{
    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Contact.Commands;

    using MediatR;

    public class CreateContactCommandHandler : IRequestHandler<CreateContactCommand, int>
    {
        private readonly IGenericRepository<Contact> contactRepository;

        public CreateContactCommandHandler(IGenericRepository<Contact> contactRepository)
        {
            this.contactRepository = contactRepository;
        }

        public int Handle(CreateContactCommand message)
        {
            var contact = new Contact
            {
                FirstName = message.FirstName,
                Surname = message.Surname,
                Title = message.Title
            };

            this.contactRepository.Add(contact);
            this.contactRepository.Save();

            return contact.Id;
        }
    }
}
