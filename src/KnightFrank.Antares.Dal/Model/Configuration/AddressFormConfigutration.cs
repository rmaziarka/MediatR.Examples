namespace KnightFrank.Antares.Dal.Migrations
{
    using KnightFrank.Antares.Dal.Model;

    internal sealed class AddressFormConfigutration : BaseEntityConfiguration<AddressForm>
    {
        public AddressFormConfigutration()
        {
            this.HasMany(p => p.AddressFieldFormDefinitions)
                .WithRequired(p => p.AddressForm)
                .WillCascadeOnDelete(false);
        }
    }
}