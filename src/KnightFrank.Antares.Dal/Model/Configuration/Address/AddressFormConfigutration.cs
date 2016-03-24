namespace KnightFrank.Antares.Dal.Model.Configuration.Address
{
    using KnightFrank.Antares.Dal.Model.Address;

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