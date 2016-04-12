namespace KnightFrank.Antares.Domain.Property.Commands
{
    using System;

    using KnightFrank.Antares.Dal.Model.Enum;

    using MediatR;

    public class CreatePropertyCommand : IRequest<Guid>
    {
        public CreateOrUpdatePropertyAddress Address { get; set; }

        public Guid PropertyTypeId { get; set; }

        public EnumTypeItem Division { get; set; }
    }
}
