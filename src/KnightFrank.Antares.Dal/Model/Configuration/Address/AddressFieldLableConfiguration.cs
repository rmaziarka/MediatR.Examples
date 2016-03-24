namespace KnightFrank.Antares.Dal.Model.Configuration.Address
{
    using KnightFrank.Antares.Dal.Model.Address;

    internal sealed class AddressFieldLableConfiguration : BaseEntityConfiguration<AddressFieldLabel>
    {
        public AddressFieldLableConfiguration()
        {
            this.Property(p => p.LabelKey).HasMaxLength(100);

            this.HasMany(p => p.AddressFieldFormDefinitions)
                .WithRequired(p => p.AddressFieldLabel)
                .WillCascadeOnDelete(false);
        }
    }
}