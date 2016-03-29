namespace KnightFrank.Antares.Domain.Ownership.Queries
{
    using System;
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Property;

    using MediatR;

    public class OwnershipByPropertyIdQuery : IRequest<IEnumerable<Ownership>>
    {
        public Guid PropertyId { get; set; }
    }
}