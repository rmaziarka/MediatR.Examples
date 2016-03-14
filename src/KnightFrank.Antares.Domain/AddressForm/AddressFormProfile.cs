namespace KnightFrank.Antares.Domain.AddressForm
{
    using AutoMapper;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Domain.AddressFieldDefinition;

    public class AddressFormProfile : Profile
    {
        protected override void Configure()
        {
            this.CreateMap<AddressFieldDefinition, AddressFieldDefinitionResult>();

            this.CreateMap<AddressForm, AddressFormQueryResult>();
        }
    }
}
