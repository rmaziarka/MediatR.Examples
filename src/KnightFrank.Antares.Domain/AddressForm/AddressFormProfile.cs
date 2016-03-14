﻿namespace KnightFrank.Antares.Domain.AddressForm
{
    using AutoMapper;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Domain.AddressFieldDefinition;

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
