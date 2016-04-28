namespace KnightFrank.Antares.Domain.Viewing.Commands
{
    using AutoMapper;
    using KnightFrank.Antares.Dal.Model.Property;

    public class ViewingCommandProfile : Profile
    {
        protected override void Configure()
        {
            this.CreateMap<CreateViewingCommand, Viewing>();
        }
    }
}