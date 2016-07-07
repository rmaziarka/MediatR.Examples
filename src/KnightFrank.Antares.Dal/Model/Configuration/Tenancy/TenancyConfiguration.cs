namespace KnightFrank.Antares.Dal.Model.Configuration.Tenancy
{
    using KnightFrank.Antares.Dal.Model.Tenancy;

    internal sealed class TenancyConfiguration : BaseEntityConfiguration<Tenancy>
    {
        public TenancyConfiguration()
        {
            this.HasMany(x => x.Terms)
                .WithRequired()
                .HasForeignKey(x => x.TenancyId)
                .WillCascadeOnDelete(false);

            this.HasMany(x => x.Landlords)
                .WithMany()
                .Map(t =>
                {
                    t.MapLeftKey("TenancyId");
                    t.MapRightKey("LandlordId");
                    t.ToTable("TenancyLandlord");
                });

            this.HasMany(x => x.Tenants)
                .WithMany()
                .Map(t =>
                {
                    t.MapLeftKey("TenancyId");
                    t.MapRightKey("TenantId");
                    t.ToTable("TenancyTenant");
                });
        }
    }
}
