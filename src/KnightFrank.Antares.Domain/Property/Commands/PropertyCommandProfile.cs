namespace KnightFrank.Antares.Domain.Property.Commands
{
    using AutoMapper;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Model.Property;

    public class PropertyCommandProfile : Profile
    {
        protected override void Configure()
        {
            this.CreateMap<UpdatePropertyCommand, Property>();
            this.CreateMap<CreatePropertyCommand, Property>();

            this.CreateMap<CreateOrUpdatePropertyAddress, Address>();
        }
    }
}