namespace KnightFrank.Antares.Dal.Migrations
{
    using KnightFrank.Antares.Dal.Model;

    internal sealed class AddressFieldConfiguration : BaseEntityConfiguration<AddressField>
    {
        public AddressFieldConfiguration()
        {
            this.Property(p => p.Name).HasMaxLength(100);

            this.HasMany(p => p.AddressFieldFormDefinitions)
                .WithRequired(p => p.AddressField)
                .WillCascadeOnDelete(false);

            this.HasMany(p => p.AddressFieldLabels)
                .WithRequired(p => p.AddressField)
                .WillCascadeOnDelete(false);
        }
    }
}