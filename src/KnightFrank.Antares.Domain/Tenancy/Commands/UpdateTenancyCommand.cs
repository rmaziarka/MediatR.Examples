namespace KnightFrank.Antares.Domain.Tenancy.Commands
{
    using System;

    public class UpdateTenancyCommand : TenancyCommandBase
    {
        public Guid TenancyId { get; set; }
    }
}