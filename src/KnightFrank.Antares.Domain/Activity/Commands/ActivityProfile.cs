namespace KnightFrank.Antares.Domain.Activity.Commands
{
    using AutoMapper;

    using KnightFrank.Antares.Dal.Model.Property.Activities;

    public class ActivityProfile : Profile
    {
        protected override void Configure()
        {
            this.CreateMap<CreateActivityCommand, Activity>();
            this.CreateMap<UpdateActivityCommand, Activity>().ForSourceMember(x => x.Id, y => y.Ignore());
        }
    }
}
