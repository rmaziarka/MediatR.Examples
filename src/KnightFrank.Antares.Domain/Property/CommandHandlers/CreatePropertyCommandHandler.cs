namespace KnightFrank.Antares.Domain.Property.CommandHandlers
{
    using System;

    using KnightFrank.Antares.Domain.Property.Commands;

    using MediatR;
    public class CreatePropertyCommandHandler : IRequestHandler<CreatePropertyCommand, Guid>
    {
        public Guid Handle(CreatePropertyCommand message)
        {
            throw new NotImplementedException();
        }
    }
}