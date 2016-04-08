namespace KnightFrank.Antares.Domain.Activity.CommandHandlers
{
    using System;

    using KnightFrank.Antares.Domain.Activity.Commands;

    using MediatR;
    public class UpdateActivityCommandHandler : IRequestHandler<UpdateActivityCommand, Guid>
    {
        public Guid Handle(UpdateActivityCommand message)
        {
            throw new NotImplementedException();
        }
    }
}