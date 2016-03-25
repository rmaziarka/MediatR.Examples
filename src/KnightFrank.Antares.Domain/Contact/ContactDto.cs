namespace KnightFrank.Antares.Domain.Contact
{
    using System;

    using AutoMapper;

    using KnightFrank.Antares.Dal.Model.Contact;

    public class ContactDto
    {
        public Guid Id { get; set; }
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
