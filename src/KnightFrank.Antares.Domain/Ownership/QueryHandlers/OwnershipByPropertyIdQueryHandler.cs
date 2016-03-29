namespace KnightFrank.Antares.Domain.Ownership.QueryHandlers
{
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Ownership.Queries;

    using MediatR;

    public class OwnershipByPropertyIdQueryHandler : IRequestHandler<OwnershipByPropertyIdQuery, IEnumerable<Ownership>>
    {
        private readonly IReadGenericRepository<Ownership> ownershipRepository;

        public OwnershipByPropertyIdQueryHandler(IReadGenericRepository<Ownership> ownershipRepository)
        {
            this.ownershipRepository = ownershipRepository;
        }

        public IEnumerable<Ownership> Handle(OwnershipByPropertyIdQuery message)
        {
            var ownerships =
                this.ownershipRepository
                    .GetWithInclude(o=>o.Contacts)
                    .Where(o=>o.PropertyId.Equals(message.PropertyId));

            return ownerships;
        }
    }
}