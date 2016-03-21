namespace KnightFrank.Antares.Domain.Property.CommandHandlers
{
    using System;

    using AutoMapper;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Property.Commands;

    using MediatR;
    public class CreatePropertyCommandHandler : IRequestHandler<CreatePropertyCommand, Property>
    {
        private readonly IGenericRepository<Property> propertyRepository;

        public CreatePropertyCommandHandler(IGenericRepository<Property> propertyRepository)
        {
            this.propertyRepository = propertyRepository;
        }

        public Property Handle(CreatePropertyCommand message)
        {
            var property = Mapper.Map<Property>(message);

            this.propertyRepository.Add(property);
            this.propertyRepository.Save();

            return property;
        }
    }
}