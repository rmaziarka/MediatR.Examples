namespace KnightFrank.Antares.Dal.Model.Configuration.Address
{
    using KnightFrank.Antares.Dal.Model.Address;

    internal sealed class AddressFieldLabelConfiguration : BaseEntityConfiguration<AddressFieldLabel>
    {
        public AddressFieldLabelConfiguration()
        {
            this.Property(p => p.LabelKey).HasMaxLength(100);
        }
    }
}