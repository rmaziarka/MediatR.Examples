namespace KnightFrank.Antares.Domain.Property.Commands
{
    using AutoMapper;

    using KnightFrank.Antares.Dal.Model;

    public class CreatePropertyCommandProfile : Profile
    {
        protected override void Configure()
        {
            this.CreateMap<CreatePropertyAddress, Address>();

            this.CreateMap<CreatePropertyCommand, Property>();
        }
    }
}