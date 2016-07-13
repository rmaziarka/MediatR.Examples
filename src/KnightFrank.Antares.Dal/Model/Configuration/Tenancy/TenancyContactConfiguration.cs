namespace KnightFrank.Antares.Dal.Model.Configuration.Tenancy
{
    using KnightFrank.Antares.Dal.Model.Tenancy;

    internal sealed class TenancyContactConfiguration : BaseEntityConfiguration<TenancyContact>
    {
        public TenancyContactConfiguration()
        {
            this.HasRequired(x => x.ContactType).WithMany().WillCascadeOnDelete(false);
            this.HasRequired(x => x.Contact).WithMany().HasForeignKey(x => x.ContactId).WillCascadeOnDelete(false);
        }
    }
}
