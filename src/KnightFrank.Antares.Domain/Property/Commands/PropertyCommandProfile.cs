namespace KnightFrank.Antares.Domain.Property.Commands
{
    using AutoMapper;

    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.Dal.Model.Attribute;
    using KnightFrank.Antares.Dal.Model.Property;

    public class PropertyCommandProfile : Profile
    {
        protected override void Configure()
        {
            this.CreateMap<UpdatePropertyCommand, Property>()
                .ForMember(dest => dest.PropertyCharacteristics, opt => opt.Ignore());

            this.CreateMap<CreatePropertyCommand, Property>();

            this.CreateMap<CreateOrUpdatePropertyAddress, Address>();
            this.CreateMap<CreateOrUpdatePropertyAttributeValues, AttributeValues>();
            this.CreateMap<CreateOrUpdatePropertyCharacteristic, PropertyCharacteristic>();
        }
    }
}