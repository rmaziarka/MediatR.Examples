namespace KnightFrank.Antares.Dal.Model.Configuration.Portal
{
    using KnightFrank.Antares.Dal.Model.Portal;

    internal class PortalConfiguration : BaseEntityConfiguration<Portal>
    {
        public PortalConfiguration()
        {
            this.Property(p => p.Name).HasMaxLength(4000);
        }
    }
}
