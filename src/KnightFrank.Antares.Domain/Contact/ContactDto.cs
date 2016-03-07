namespace KnightFrank.Antares.Domain.Contact
{
    using AutoMapper;

    using KnightFrank.Antares.Dal.Model;

    public class ContactDto
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Title { get; set; }
    }

    public class ContactDtoProfile : Profile
    {
        protected override void Configure()
        {
            this.CreateMap<ContactDto, Contact>();
        }
    }

    public class ContactProfile : Profile
    {
        protected override void Configure()
        {
            this.CreateMap<Contact, ContactDto>();
        }
    }
}
