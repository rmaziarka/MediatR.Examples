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
        private readonly IGenericRepository<PropertyAreaBreakdown> propertyAreaBreakdownRepository;

        private readonly IEntityValidator entityValidator;

        private readonly IPropertyAreaBreakdownValidator propertyAreaBreakdownValidator;

        private readonly IGenericRepository<Property> propertyRepository;

        public UpdateAreaBreakdownCommandHandler(
            IEntityValidator entityValidator,
            IPropertyAreaBreakdownValidator propertyAreaBreakdownValidator,
            IGenericRepository<PropertyAreaBreakdown> propertyAreaBreakdownRepository,
            IGenericRepository<Property> propertyRepository)
        {
            this.entityValidator = entityValidator;
            this.propertyAreaBreakdownValidator = propertyAreaBreakdownValidator;
            this.propertyAreaBreakdownRepository = propertyAreaBreakdownRepository;
            this.propertyRepository = propertyRepository;
        }

        public Guid Handle(UpdateAreaBreakdownCommand command)
        {
            Property property = this.propertyRepository.GetById(command.PropertyId);
            PropertyAreaBreakdown propertyAreaBreakdown = this.propertyAreaBreakdownRepository.GetById(command.Id);

            this.entityValidator.EntityExists(property, command.PropertyId);
            this.entityValidator.EntityExists(propertyAreaBreakdown, command.Id);
            this.propertyAreaBreakdownValidator.IsAssignToProperty(propertyAreaBreakdown, property);

            Mapper.Map(command, propertyAreaBreakdown);

            this.propertyAreaBreakdownRepository.Save();

            return propertyAreaBreakdown.Id;
        }
    }
}
