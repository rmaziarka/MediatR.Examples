namespace KnightFrank.Antares.Domain.Tenancy.Commands
{
    using System;
    using System.Collections.Generic;

    public class CreateTenancyCommand : TenancyCommandBase
    {
        public Guid ActivityId { get; set; }

        public Guid RequirementId { get; set; }

        public IList<Guid> LandlordContacts { get; set; } = new List<Guid>();

        public IList<Guid> TenantContacts { get; set; } = new List<Guid>();
    }
}