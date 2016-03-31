namespace KnightFrank.Antares.Domain.Property.Commands
{
    using System;

    using MediatR;

    public class CreatePropertyCommand : IRequest<Guid>
    {
        public CreateOrUpdatePropertyAddress Address { get; set; }

        public Guid PropertyTypeId { get; set; }
    }
}
