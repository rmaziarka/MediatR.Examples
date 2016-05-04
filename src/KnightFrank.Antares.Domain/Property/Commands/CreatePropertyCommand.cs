namespace KnightFrank.Antares.Domain.Property.Commands
{
    using System;
    using System.Collections.Generic;

    using KnightFrank.Antares.Domain.Common.Commands;

    using MediatR;

    public class CreatePropertyCommand : IRequest<Guid>
    {
        public CreateOrUpdateAddress Address { get; set; }

        public Guid PropertyTypeId { get; set; }

        public Guid DivisionId { get; set; }

        public CreateOrUpdatePropertyAttributeValues AttributeValues { get; set; }

        public IList<CreateOrUpdatePropertyCharacteristic> PropertyCharacteristics { get; set; }
    }
}
