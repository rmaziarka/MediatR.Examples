namespace KnightFrank.Antares.Domain.Property.Commands
{
    using System;
    using System.Collections.Generic;

    using MediatR;

    public class UpdatePropertyCommand : IRequest<Guid>
    {
        private IList<CreateOrUpdatePropertyCharacteristic> propertyCharacteristics;
        public Guid Id { get; set; }

        public CreateOrUpdatePropertyAddress Address { get; set; }

        public Guid PropertyTypeId { get; set; }

        public Guid DivisionId { get; set; }

        public CreateOrUpdatePropertyAttributeValues AttributeValues { get; set; }

        public IList<CreateOrUpdatePropertyCharacteristic> PropertyCharacteristics 
        {
            get { return this.propertyCharacteristics ?? new List<CreateOrUpdatePropertyCharacteristic>(); }
            set { this.propertyCharacteristics = value; }
        }
    }
}