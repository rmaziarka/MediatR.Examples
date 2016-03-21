namespace KnightFrank.Antares.Domain.Property.Commands
{
    using System;

    using MediatR;

    public class CreatePropertyCommand : IRequest<Guid>
    {
        public CreatePropertyAddress Address { get; set; }
    }
}
