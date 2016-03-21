namespace KnightFrank.Antares.Domain.Property.Commands
{
    using AutoMapper;

    using KnightFrank.Antares.Dal.Model;

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