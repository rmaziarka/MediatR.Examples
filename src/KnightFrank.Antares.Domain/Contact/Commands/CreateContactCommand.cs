namespace KnightFrank.Antares.Domain.Contact.Commands
{
    using MediatR;

    public class CreateContactCommand : IRequest<int>
    {
        public string FirstName { get; set; }

        public string Surname { get; set; }

        public string Title { get; set; }
    }
}
