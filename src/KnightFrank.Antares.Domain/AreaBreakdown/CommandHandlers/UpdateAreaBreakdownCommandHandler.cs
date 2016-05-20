namespace KnightFrank.Antares.Domain.AreaBreakdown.CommandHandlers
{
    using System;

    using AutoMapper;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.AreaBreakdown.Commands;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;

    using MediatR;

    public class UpdateAreaBreakdownCommandHandler : IRequestHandler<UpdateAreaBreakdownCommand, Guid>
    {
        private readonly IGenericRepository<PropertyAreaBreakdown> areaBreakdownRepository;

        private readonly IEntityValidator entityValidator;

        private readonly IGenericRepository<Property> propertyRepository;

        public UpdateAreaBreakdownCommandHandler(
            IEntityValidator entityValidator,
            IGenericRepository<PropertyAreaBreakdown> areaBreakdownRepository,
            IGenericRepository<Property> propertyRepository)
        {
            this.entityValidator = entityValidator;
            this.areaBreakdownRepository = areaBreakdownRepository;
            this.propertyRepository = propertyRepository;
        }

        public Guid Handle(UpdateAreaBreakdownCommand command)
        {
            Property property = this.propertyRepository.GetById(command.PropertyId);
            PropertyAreaBreakdown areaBreakdown = this.areaBreakdownRepository.GetById(command.Id);

            this.entityValidator.EntityExists(property, command.PropertyId);
            this.entityValidator.EntityExists(areaBreakdown, command.Id);

            Mapper.Map(command, areaBreakdown);

            this.areaBreakdownRepository.Save();

            return areaBreakdown.Id;
        }
    }
}
