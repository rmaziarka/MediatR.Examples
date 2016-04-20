namespace KnightFrank.Antares.Dal.Model.Configuration.Address
{
    using KnightFrank.Antares.Dal.Model.Address;

    internal sealed class AddressFieldConfiguration : BaseEntityConfiguration<AddressField>
    {
        public AddressFieldConfiguration()
        {
            this.Property(p => p.Name)
                .HasMaxLength(100)
                .IsUnique()
                .IsRequired();

            this.HasMany(p => p.AddressFieldLabels)
                .WithRequired(p => p.AddressField)
                .WillCascadeOnDelete(false);
        }
    }
}