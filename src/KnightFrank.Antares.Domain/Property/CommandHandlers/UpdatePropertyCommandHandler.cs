namespace KnightFrank.Antares.Domain.Property.CommandHandlers
{
    using System;

    using AutoMapper;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.Exceptions;
    using KnightFrank.Antares.Domain.Property.Commands;

    using MediatR;

    public class UpdatePropertyCommandHandler : IRequestHandler<UpdatePropertyCommand, Guid>
    {
        private readonly IGenericRepository<Property> propertyRepository;

        public UpdatePropertyCommandHandler(
            IGenericRepository<Property> propertyRepository)
        {
            this.propertyRepository = propertyRepository;
        }

        public Guid Handle(UpdatePropertyCommand message)
        {
            Property property = this.propertyRepository.GetById(message.Id);

            if (property == null)
            {
                throw new ResourceNotFoundException("Property does not exist", message.Id);
            }

            Mapper.Map(message.Address, property.Address);

            this.propertyRepository.Save();

            return property.Id;
        }
    }
}