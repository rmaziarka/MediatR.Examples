namespace KnightFrank.Antares.Domain.Contact.Commands
{
    using AutoMapper;

    using KnightFrank.Antares.Dal.Model.Contacts;

    public class UpdateContactCommandProfile : Profile
    {
        protected override void Configure()
        {
            this.CreateMap<UpdateContactCommand, Contact>();
        }
    }
}
