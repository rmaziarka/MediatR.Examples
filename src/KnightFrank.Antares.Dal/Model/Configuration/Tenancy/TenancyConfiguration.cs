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
        }
    }
}
