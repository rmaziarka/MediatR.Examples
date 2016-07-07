namespace KnightFrank.Antares.Dal.Model.Configuration.Tenancy
{
    using KnightFrank.Antares.Dal.Model.Tenancy;

    internal sealed class TenancyTermConfiguration : BaseEntityConfiguration<TenancyTerm>
    {
        public TenancyTermConfiguration()
        {
            this.Property(x => x.Price).IsMoney().IsRequired();
            this.Property(x => x.StartDate).IsRequired();
            this.Property(x => x.EndDate).IsRequired();
        }
    }
}
