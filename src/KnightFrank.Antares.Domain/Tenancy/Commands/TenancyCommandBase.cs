namespace KnightFrank.Antares.Domain.Tenancy.Commands
{
    using System;

    using MediatR;

    public class TenancyCommandBase : IRequest<Guid>
    {
        public UpdateTenancyTerm Term { get; set; } = new UpdateTenancyTerm();
    }
}