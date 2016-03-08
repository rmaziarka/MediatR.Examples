namespace KnightFrank.Antares.Domain.Contact.Commands
{
    using System;

    using MediatR;

    public class CreateContactCommand : IRequest<Guid>
    {
        public string FirstName { get; set; }

        public string Surname { get; set; }

        public string Title { get; set; }
    }
}
