namespace KnightFrank.Antares.Dal.Model.Configuration.Company
{
    using KnightFrank.Antares.Dal.Model.Company;

    internal sealed class CompanyConfiguration : BaseEntityConfiguration<Company>
    {
        public CompanyConfiguration()
        {
            this.Property(p => p.Name).HasMaxLength(128).IsRequired();
        }
    }
}