namespace KnightFrank.Antares.Domain.Contact.Queries
{
    using System;

    using KnightFrank.Antares.Dal.Model.Contacts;

    using MediatR;

    public class ContactQuery : IRequest<Contact>
    {
        public Guid Id { get; set; }
    }
}
