namespace KnightFrank.Antares.Domain.Property.Commands
{
    using System;

    using MediatR;

    public class UpdatePropertyCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }

        public CreateOrUpdatePropertyAddress Address { get; set; }

        public Guid PropertyTypeId { get; set; }

        public Guid DivisionId { get; set; }

        public CreateOrUpdatePropertyAttributeValues AttributeValues { get; set; }
    }
}