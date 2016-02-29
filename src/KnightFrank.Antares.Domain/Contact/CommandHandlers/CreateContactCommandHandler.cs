namespace KnightFrank.Antares.Domain.Contact.CommandHandlers
{
    using KnightFrank.Antares.Domain.Contact.Commands;

    using MediatR;

    public class CreateContactCommandHandler: IRequestHandler<CreateContactCommand, int>
    {
        public int Handle(CreateContactCommand message)
        {
            return 3;
        }
    }
}
