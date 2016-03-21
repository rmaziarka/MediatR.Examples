namespace KnightFrank.Antares.Domain.Requirement.Commands
{
    using System;
    using System.Collections.Generic;

    using AutoMapper;

    using Dal.Model;
    using Contact;

    using MediatR;

    public class CreateRequirementCommand : IRequest<Requirement>
    {
        public DateTime CreateDate { get; set; }

        public ICollection<ContactDto> Contacts { get; set; }

        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public int? MinBedrooms { get; set; }
        public int? MaxBedrooms { get; set; }

        public int? MinReceptionRooms { get; set; }
        public int? MaxReceptionRooms { get; set; }

        public int? MinBathrooms { get; set; }
        public int? MaxBathrooms { get; set; }

        public int? MinParkingSpaces { get; set; }
        public int? MaxParkingSpaces { get; set; }

        public double? MinArea { get; set; }
        public double? MaxArea { get; set; }

        public double? MinLandArea { get; set; }
        public double? MaxLandArea { get; set; }

        public string Description { get; set; }
    }

    public class CreateRequirementCommandProfile : Profile
    {
        protected override void Configure()
        {
            this.CreateMap<CreateRequirementCommand, Requirement>();
        }
    }
}