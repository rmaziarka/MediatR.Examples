namespace KnightFrank.Antares.Domain.Activity.Commands
{
    using AutoMapper;

    using KnightFrank.Antares.Dal.Model.Property;

    public class UpdateActivityCommandProfile : Profile
    {
        protected override void Configure()
        {
            this.CreateMap<UpdateActivityCommand, Activity>().ForSourceMember(x => x.ActivityId, y => y.Ignore());
        }
    }
}