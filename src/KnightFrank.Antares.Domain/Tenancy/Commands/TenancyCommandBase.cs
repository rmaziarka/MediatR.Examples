namespace KnightFrank.Antares.Domain.Tenancy.Commands
{
    using System;
    using System.Collections.Generic;

    using MediatR;

    public class TenancyCommandBase : IRequest<Guid>
    {
        public Guid ActivityId { get; set; }

        public Guid RequirementId { get; set; }

        public IList<CreateTenancyTerm> Terms { get; set; } = new List<CreateTenancyTerm>();

        public IList<Guid> Landlords { get; set; } = new List<Guid>();

        public IList<Guid> Tenants { get; set; } = new List<Guid>();
    }
}