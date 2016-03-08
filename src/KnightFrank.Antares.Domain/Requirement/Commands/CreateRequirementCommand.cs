namespace KnightFrank.Antares.Domain.Requirement.Commands
{
    using System;
    using System.Collections.Generic;

    using AutoMapper;

    using Dal.Model;
    using Contact;

    using MediatR;

    public class CreateRequirementCommand : IRequest<int>
    {
        public DateTime CreateDate { get; set; }

        public ICollection<ContactDto> Contacts { get; set; }
    }

    public class CreateRequirementCommandProfile : Profile
    {
        protected override void Configure()
        {
            this.CreateMap<CreateRequirementCommand, Requirement>();
        }
    }
}