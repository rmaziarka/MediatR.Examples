namespace KnightFrank.Antares.Domain.Contact.Queries
{
    using System;

    using KnightFrank.Antares.Dal.Model.Contact;

    using MediatR;

    public class ContactQuery : IRequest<Contact>
    {
        public Guid Id { get; set; }
    }
}
