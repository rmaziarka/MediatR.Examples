namespace KnightFrank.Antares.Dal.Migrations
{
    using System.Data.Entity.ModelConfiguration;

    using KnightFrank.Antares.Dal.Model;

    internal sealed class AddressFieldDefinitionConfiguration : BaseEntityConfiguration<AddressFieldDefinition>
    {
        public AddressFieldDefinitionConfiguration()
        {
            this.Property(p => p.RegEx).HasMaxLength(50);
        }
    }
}