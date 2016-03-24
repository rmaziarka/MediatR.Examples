namespace KnightFrank.Antares.Dal.Model.Configuration.Address
{
    using KnightFrank.Antares.Dal.Model.Address;

    internal sealed class AddressFieldDefinitionConfiguration : BaseEntityConfiguration<AddressFieldDefinition>
    {
        public AddressFieldDefinitionConfiguration()
        {
            this.Property(p => p.RegEx).HasMaxLength(50);

            this.HasRequired(p => p.AddressField)
                .WithMany()
                .WillCascadeOnDelete(false);

            this.HasRequired(p => p.AddressFieldLabel)
                .WithMany()
                .WillCascadeOnDelete(false);
        }
    }
}
