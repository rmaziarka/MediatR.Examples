namespace KnightFrank.Antares.Domain.Ownership.Queries
{
    using System;

    using KnightFrank.Antares.Dal.Model.Property;

    using MediatR;

    public class OwnershipByIdQuery : IRequest<Ownership>
    {
        public Guid OwnershipId { get; set; }
    }
}