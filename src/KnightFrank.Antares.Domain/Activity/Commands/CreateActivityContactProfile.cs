namespace KnightFrank.Antares.Domain.Activity.Commands
{
    using AutoMapper;

    using KnightFrank.Antares.Dal.Model.Contacts;

    public class CreateActivityContactProfile : Profile
    {
        protected override void Configure()
        {
            this.CreateMap<CreateActivityContact, Contact>();
        }
    }
}