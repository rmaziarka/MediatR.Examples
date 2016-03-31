namespace KnightFrank.Antares.Domain.Ownership.QueryHandlers
{
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Ownership.Queries;

    using MediatR;

    public class OwnershipByIdQueryHandler : IRequestHandler<OwnershipByIdQuery, Ownership>
    {
        private readonly IReadGenericRepository<Ownership> ownershipRepository;

        public OwnershipByIdQueryHandler(IReadGenericRepository<Ownership> ownershipRepository)
        {
            this.ownershipRepository = ownershipRepository;
        }

        public Ownership Handle(OwnershipByIdQuery message)
        {
            var ownership =
                this.ownershipRepository
                    .GetWithInclude(o=>o.Contacts, o => o.OwnershipType)
                    .First(o=>o.Id.Equals(message.OwnershipId));

            return ownership;
        }
    }
}