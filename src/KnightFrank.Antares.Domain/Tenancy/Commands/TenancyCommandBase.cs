namespace KnightFrank.Antares.Domain.Tenancy.Commands
{
    using System;
    using System.Collections.Generic;

    using MediatR;

    public class TenancyCommandBase : IRequest<Guid>
    {
        public IList<CreateTenancyTerm> Terms { get; set; } = new List<CreateTenancyTerm>();
    }
}