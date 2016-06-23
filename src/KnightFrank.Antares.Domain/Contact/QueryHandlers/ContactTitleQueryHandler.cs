namespace KnightFrank.Antares.Domain.Contact.QueryHandlers
{
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Contact.Queries;

    using MediatR;

    public class ContactTitleQueryHandler : IRequestHandler<ContactTitleQuery, IEnumerable<ContactTitle>>
    {
        private readonly IReadGenericRepository<ContactTitle> contactTitletRepository;

        public ContactTitleQueryHandler(IReadGenericRepository<ContactTitle> contactTitletRepository)
        {
            this.contactTitletRepository = contactTitletRepository;
        }

        public IEnumerable<ContactTitle> Handle(ContactTitleQuery message)
        {
            return this.contactTitletRepository.GetWithInclude(x => x.Locale).ToList();
        }
    }
}
