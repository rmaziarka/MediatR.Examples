namespace KnightFrank.Antares.Domain.AreaBreakdown.Commands
{
    using AutoMapper;

    using KnightFrank.Antares.Dal.Model.Property;

    public class AreaBreakdownProfile : Profile
    {
        protected override void Configure()
        {
            this.CreateMap<UpdateAreaBreakdownCommand, PropertyAreaBreakdown>()
                .ForMember(x => x.PropertyId, options => options.Ignore());
        }
    }
}