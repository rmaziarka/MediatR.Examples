namespace KnightFrank.Antares.Domain.AreaBreakdown.Commands
{
    using AutoMapper;

    using KnightFrank.Antares.Dal.Model.Property;

    public class UpdateAreaBreakdownCommandProfile : Profile
    {
        protected override void Configure()
        {
            this.CreateMap<UpdateAreaBreakdownCommand, PropertyAreaBreakdown>()
                .ForMember(a => a.Id, x => x.Ignore())
                .ForMember(a => a.PropertyId, x => x.Ignore());
        }
    }
}
