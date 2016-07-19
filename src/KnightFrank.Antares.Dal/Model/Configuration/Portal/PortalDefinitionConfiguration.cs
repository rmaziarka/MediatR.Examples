namespace KnightFrank.Antares.Dal.Model.Configuration.Portal
{
    using KnightFrank.Antares.Dal.Model.Portal;
    internal class PortalDefinitionConfiguration:BaseEntityConfiguration<PortalDefinition>
    {
        public PortalDefinitionConfiguration()
        {
            this.Property(p => p.CountryId).IsRequired();
            this.Property(p => p.PortalId).IsRequired();
        }
    }
}
