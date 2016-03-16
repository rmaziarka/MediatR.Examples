namespace KnightFrank.Antares.Dal.Migrations
{
    using KnightFrank.Antares.Dal.Model;

    internal sealed class AddressFormConfigutration : BaseEntityConfiguration<AddressForm>
    {
        public AddressFormConfigutration()
        {
            this.HasMany(p => p.AddressFieldDefinitions)
                .WithRequired(p => p.AddressForm)
                .WillCascadeOnDelete(false);
        }
    }
}