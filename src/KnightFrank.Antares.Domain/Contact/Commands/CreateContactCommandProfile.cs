namespace KnightFrank.Antares.Domain.Contact.Commands
{
    using AutoMapper;

    using KnightFrank.Antares.Dal.Model;

    public class CreateContactCommandProfile : Profile
    {
        protected override void Configure()
        {
            this.CreateMap<CreateContactCommand, Contact>();
        }
    }
}
