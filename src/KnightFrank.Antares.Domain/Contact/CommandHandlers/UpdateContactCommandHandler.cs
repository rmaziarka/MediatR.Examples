namespace KnightFrank.Antares.Domain.Contact.CommandHandlers
{
    using KnightFrank.Antares.Domain.Contact.Commands;

    using MediatR;

    public class UpdateContactCommandHandler : RequestHandler<UpdateContactCommand>
    {
        protected override void HandleCore(UpdateContactCommand message)
        {
            //do nothing
        }
    }
}
