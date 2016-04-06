namespace KnightFrank.Antares.Domain.Activity.Commands
{
    using AutoMapper;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Property;

    public class CreateActivityCommandProfile : Profile
    {
        protected override void Configure()
        {
            this.CreateMap<CreateActivityCommand, Activity>();
            this.CreateMap<CreateActivityContact, Contact>();
        }
    }
}
