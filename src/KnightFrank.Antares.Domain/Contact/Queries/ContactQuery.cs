namespace KnightFrank.Antares.Domain.Contact.Queries
{
    using System;

    using KnightFrank.Antares.Dal.Model;

    using MediatR;

    public class ContactQuery : IRequest<Contact>
    {
        public Guid Id { get; set; }
    }
}
