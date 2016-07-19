namespace KnightFrank.Antares.Dal.Model.Portal
{
    using System;

    using KnightFrank.Antares.Dal.Model.Resource;

    public class PortalDefinition : BaseEntity
    {
        public Country Country { get; set; }
        public Guid CountryId { get; set; }

        public Portal Portal { get; set; }
        public Guid PortalId { get; set; }
    }
}
