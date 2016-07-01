namespace KnightFrank.Antares.Domain.Contact.Commands
{
    using System;
    using MediatR;

    public class UpdateContactCommand : IRequest<Guid> 
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Title { get; set; }
    }
}
