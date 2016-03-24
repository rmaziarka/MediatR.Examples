namespace KnightFrank.Antares.Domain.AddressForm.QueryResults
{
    using AutoMapper;

    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.Domain.AddressFieldDefinition.QueryResults;

    public class AddressFormProfile : Profile
    {
        protected override void Configure()
        {
            this.CreateMap<AddressFieldDefinition, AddressFieldDefinitionResult>()
                .ForMember(r => r.Name, opt => opt.MapFrom(s => s.AddressField.Name))
                .ForMember(r => r.LabelKey, opt => opt.MapFrom(s => s.AddressFieldLabel.LabelKey));

            this.CreateMap<AddressForm, AddressFormQueryResult>();
        }
    }
}
