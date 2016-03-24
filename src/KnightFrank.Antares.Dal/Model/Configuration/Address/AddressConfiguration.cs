namespace KnightFrank.Antares.Dal.Model.Configuration.Address
{
    using KnightFrank.Antares.Dal.Model.Address;

    internal sealed class AddressConfiguration : BaseEntityConfiguration<Address>
    {
        public AddressConfiguration()
        {
            this.Property(p => p.PropertyName).HasMaxLength(28);
            this.Property(p => p.PropertyNumber).HasMaxLength(8);
            this.Property(p => p.Line1).HasMaxLength(128);
            this.Property(p => p.Line2).HasMaxLength(128);
            this.Property(p => p.Line3).HasMaxLength(128);
            this.Property(p => p.Postcode).HasMaxLength(10);
            this.Property(p => p.City).HasMaxLength(128);
            this.Property(p => p.County).HasMaxLength(128);
        }
    }
}
