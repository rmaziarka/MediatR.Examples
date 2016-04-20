namespace KnightFrank.Antares.Domain.Activity.Commands
{
    using AutoMapper;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;

    public class CreateActivityCommandProfile : Profile
    {
        protected override void Configure()
        {
            this.CreateMap<CreateActivityCommand, Activity>();
        }
    }
}
