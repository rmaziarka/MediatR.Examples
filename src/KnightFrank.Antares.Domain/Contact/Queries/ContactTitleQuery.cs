namespace KnightFrank.Antares.Domain.Contact.Queries
{
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Contacts;

    using MediatR;

    public class ContactTitleQuery : IRequest<IEnumerable<ContactTitle>>
    {

    }
}
